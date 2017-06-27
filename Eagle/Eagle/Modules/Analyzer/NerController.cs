using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eagle.Models;
using Eagle.DbContexts;
using System.Text.RegularExpressions;
using Eagle.Utility;

namespace Eagle.Modules.Analyzer
{
    [Route("v1/Analyzer")]
    public class NerController : ControllerBase
    {
        private readonly DataContexts _context = new DataContexts();

        /// <summary>
        /// NER - 命名实体识别
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        // GET: v1/Analyzer/Ner/?
        [HttpGet("Ner/{text}")]
        public List<IntentExpressionItemModel> Ner([FromRoute] string text)
        {
            // 识别所有单元实体
            var unitEntitiesQueryable = (from entity in _context.Entities
                            join entry in _context.EntityEntries on entity.Id equals entry.EntityId
                            from synonym in _context.EntityEntrySynonyms.Where(x => x.EntityEntryId == entry.Id && text.Contains(x.Synonym)).DefaultIfEmpty()
                            where text.Contains(entry.Value) || text.Contains(synonym.Synonym)
                            select new IntentExpressionItemModel
                            {
                                EntryId = entry.Id,
                                EntityId = entity.Id,
                                Text = synonym != null ? synonym.Synonym : entry.Value,
                                Meta = $"@{entity.Name}",
                                Alias = entity.Name
                            }).ToList();

            var unitEntities = Process(text, unitEntitiesQueryable);

            string template = String.Concat(unitEntities.Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta).ToArray());
            // 识别组合实体
            var compositEntitiesQueryable = (from entity in _context.Entities
                                        join entry in _context.EntityEntries on entity.Id equals entry.EntityId
                                        where entity.IsEnum && template.Contains(entry.Value)
                                        select new IntentExpressionItemModel
                                        {
                                            EntityId = entity.Id,
                                            Text = entry.Value,
                                            Meta = $"@{entity.Name}",
                                            Alias = entity.Name
                                        }).ToList();

            var compositEntities = Process(template, compositEntitiesQueryable).Where(x => x.EntityId != null).ToList();

            

            int pos = 0;

            compositEntities.ForEach(comEntity =>
            {
                pos = CorrectPosition(comEntity, unitEntities, pos);
            });

            List<IntentExpressionItemModel> merged = compositEntities;

            // Merge unit and composite
            for (int idx = 0; idx < unitEntities.Count; idx++)
            {
                var unitEntity = unitEntities[idx];

                var list = compositEntities.Where(comEntity => unitEntity.Position >= comEntity.Position && (unitEntity.Length + unitEntity.Position) <= (comEntity.Length + comEntity.Position)).ToList();
                if (list.Count() == 0)
                {
                    merged.Add(unitEntity);
                }
            }

            return merged.OrderBy(x => x.Position).ToList();
        }

        private int CorrectPosition(IntentExpressionItemModel compositedEntity, List<IntentExpressionItemModel> unitEntities, int pos)
        {
            var source = unitEntities.Where(x => !String.IsNullOrEmpty(x.Meta) && compositedEntity.Text.Contains(x.Meta)).Select(x => x.Meta).Distinct().ToList();

            for (; pos < unitEntities.Count;)
            {
                var target = unitEntities.Select(x => new { Meta = x.Meta == null ? x.Text : x.Meta, x.Position, x.Text, x.Length })
                    .Skip(pos).Take(source.Count).ToList();
                var join = (from s in source
                            join t in target on s equals t.Meta
                            select t).OrderBy(x => x.Position).ToList();

                if(join.Count == source.Count)
                {
                    compositedEntity.Position = join.First().Position;
                    compositedEntity.Length = join.Sum(x => x.Length);
                    compositedEntity.Text = String.Concat(join.Select(x => x.Text));
                    pos += source.Count;

                    break;
                }
                else
                {
                    pos++;
                }
            }

            return pos;
        }

        private bool CheckToken(IntentExpressionItemModel compositedEntity, List<IntentExpressionItemModel> unitEntities, int idx)
        {
            var source = compositedEntity.Text.Split(' ')
                .ToList()
                .Select(x => x).ToList();

            var target = unitEntities.Select(x => x.Meta == null ? x.Text : x.Meta).Skip(idx).Take(source.Count).ToList();

            var join = (from s in source
                       join t in target on s equals t
                       select s).ToList();

            return join.Count == source.Count;
        }

        /// <summary>
        /// 按顺序返回实体数组和未识别的实体，计算实体在句子中的位置。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        private List<IntentExpressionItemModel> Process(string text, List<IntentExpressionItemModel> entities)
        {
            var tags = new List<IntentExpressionItemModel>();

            entities.ForEach(token =>
            {
                MatchCollection mc = Regex.Matches(text, token.Text);
                foreach (Match m in mc)
                {
                    IntentExpressionItemModel temp = new IntentExpressionItemModel()
                    {
                        Position = m.Index,
                        Length = m.Length,
                        EntityId = token.EntityId,
                        Alias = token.Alias,
                        Text = token.Text,
                        Meta = token.Meta,
                        Id = token.Id,
                        EntryId = token.EntryId,
                        IntentExpressionId = token.IntentExpressionId
                    };

                    tags.Add(temp);
                }
            });

            tags = tags.OrderBy(x => x.Position).ToList();

            var results = new List<IntentExpressionItemModel>();

            // 扫描字符串
            for(int pos = 0; pos < text.Length;)
            {
                var tag = tags.FirstOrDefault(x => x.Position == pos);
                if(tag != null)
                {
                    results.Add(tag);
                    pos += tag.Length;
                }
                else
                {
                    // 取下一个, 如果没找到，就一直取到最后一个字符。
                    tag = tags.FirstOrDefault(x => x.Position > pos);
                    int length = tag == null ? text.Length - pos : tag.Position - pos;

                    var item = new IntentExpressionItemModel
                    {
                        Text = text.Substring(pos, length),
                        Position = pos,
                        Length = length
                    };

                    results.Add(item);

                    pos += length;
                }
            }

            return results;
        }
    }
}