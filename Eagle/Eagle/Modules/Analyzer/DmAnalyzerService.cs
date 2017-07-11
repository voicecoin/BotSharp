using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.DomainModels;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eagle.DmServices
{
    public static class DmAnalyzerService
    {
        /// <summary>
        /// 分词与词性标注,  Part-Of-Speech Tagger (POS Tagger) 
        /// </summary>
        /// <param name="analyzerModel"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        public static List<DmIntentExpressionItem> PosTagger(this DmAgentRequest analyzerModel, DataContexts dc)
        {
            string text = analyzerModel.Text;

            // 识别所有单元实体
            // 用Entry识别
            var unitEntityInEntry = (from entity in dc.Entities
                                         join entry in dc.EntityEntries on entity.Id equals entry.EntityId
                                         where text.Contains(entry.Value)
                                         select new DmIntentExpressionItem
                                         {
                                             EntryId = entry.Id,
                                             EntityId = entity.Id,
                                             Text = entry.Value,
                                             Meta = $"@{entity.Name}",
                                             Alias = entity.Name,
                                             Length = entry.Value.Length
                                         }).ToList();

            // 用Synonym识别一次
            var unitEntityInSynonym = (from entity in dc.Entities
                                       join entry in dc.EntityEntries on entity.Id equals entry.EntityId
                                       join synonym in dc.EntityEntrySynonyms on entry.Id equals synonym.EntityEntryId
                                       where text.Contains(synonym.Synonym)
                                       select new DmIntentExpressionItem
                                       {
                                           EntryId = entry.Id,
                                           EntityId = entity.Id,
                                           Text = synonym.Synonym,
                                           Meta = $"@{entity.Name}",
                                           Alias = entity.Name,
                                           Length = entry.Value.Length
                                       }).ToList();

            var unitEntityTotal = new List<DmIntentExpressionItem>();
            unitEntityTotal.AddRange(unitEntityInEntry);
            unitEntityTotal.AddRange(unitEntityInSynonym);

            var unitEntities = Process(text, unitEntityTotal);

            string template = unitEntities.GetTemplateString(); // String.Concat(unitEntities.Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta).ToArray());
            // 识别组合实体
            var compositEntitiesQueryable = (from entity in dc.Entities
                                             join entry in dc.EntityEntries on entity.Id equals entry.EntityId
                                             where entity.IsEnum && template.Contains(entry.Value)
                                             select new DmIntentExpressionItem
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

            List<DmIntentExpressionItem> merged = compositEntities;

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

        public static List<DmIntentExpressionItem> Segment(this DmAgentRequest analyzerModel, DataContexts dc)
        {
            var taggers = PosTagger(analyzerModel, dc);

            List<DmIntentExpressionItem> segments = new List<DmIntentExpressionItem>();
            segments.AddRange(taggers.Where(x => !String.IsNullOrEmpty(x.EntityId)));

            taggers.Where(x => String.IsNullOrEmpty(x.EntityId))
                .ToList()
                .ForEach(tag =>
                {
                    var chars = tag.Text.ToCharArray();
                    for (int idx = 0; idx < chars.Length; idx++)
                    {
                        segments.Add(new DmIntentExpressionItem
                        {
                            Position = tag.Position + idx,
                            Length = 1,
                            Text = chars[idx].ToString()
                        });
                    }
                });


            return segments;
        }

        public static string GetTemplateString(this IEnumerable<DmIntentExpressionItem> items)
        {
            return String.Concat(items.Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta).ToArray());
        }

        /// <summary>
        /// 机器人回复进一步处理，替换变量，填充参数
        /// </summary>
        /// <param name="responseModel"></param>
        /// <param name="dc"></param>
        public static DmIntentResponseMessage PostResponse(this DmIntentResponse responseModel, DataContexts dc, DmAgentRequest agentRequestModel)
        {
            // 随机选择一个回答。
            DmIntentResponseMessage messageModel = responseModel.Messages.Random();

            // Replace system token
            messageModel.ReplaceSystemToken(dc, agentRequestModel);
            messageModel.ReplaceParameterToken(dc, agentRequestModel, responseModel);

            return messageModel;
        }

        public static void ExtractParameter(this DmIntentResponse responseModel, DataContexts dc, DmAgentRequest agentRequestModel)
        {
            var segments = agentRequestModel.Segment(dc).Where(x => !String.IsNullOrEmpty(x.EntityId)).ToList();

            responseModel.Parameters.ForEach(parameter =>
            {
                parameter.Value = segments.First(x => x.EntityId == parameter.EntityId).Text;
            });
        }

        /// <summary>
        /// 转换系统内置变量
        /// </summary>
        /// <param name="messageModel"></param>
        /// <param name="dc"></param>
        /// <param name="agent"></param>
        public static void ReplaceSystemToken(this DmIntentResponseMessage messageModel, DataContexts dc, DmAgentRequest agentRequestModel)
        {
            List<String> speechs = new List<string>();

            messageModel.Speech.ForEach(speech => {
                speech = speech.Replace("{@agent.name}", agentRequestModel.Agent.Name);
                speech = speech.Replace("{@agent.description}", agentRequestModel.Agent.Description);

                TimeSpan age = DateTime.UtcNow - agentRequestModel.Agent.CreatedDate;
                speech = speech.Replace("{@agent.age}", $"我刚出生{(int)age.TotalDays}天");

                speechs.Add(speech);
            });

            messageModel.Speech = speechs;
        }

        /// <summary>
        /// 转换识别实体参数
        /// </summary>
        /// <param name="messageModel"></param>
        /// <param name="dc"></param>
        /// <param name="agent"></param>
        public static void ReplaceParameterToken(this DmIntentResponseMessage messageModel, DataContexts dc, DmAgentRequest agentRequestModel, DmIntentResponse responseModel)
        {
            List<String> speechs = new List<string>();

            messageModel.Speech.ForEach(speech => {
                responseModel.Parameters.ForEach(parameter => {
                    speech = speech.Replace("{$" + parameter.Name + "}", parameter.Value);
                });
                speechs.Add(speech);
            });

            messageModel.Speech = speechs;
        }

        private static int CorrectPosition(DmIntentExpressionItem compositedEntity, List<DmIntentExpressionItem> unitEntities, int pos)
        {
            var source = unitEntities.Where(x => !String.IsNullOrEmpty(x.Meta) && compositedEntity.Text.Contains(x.Meta)).Select(x => x.Meta).Distinct().ToList();

            for (; pos < unitEntities.Count;)
            {
                var target = unitEntities.Select(x => new { Meta = x.Meta == null ? x.Text : x.Meta, x.Position, x.Text, x.Length })
                    .Skip(pos).Take(source.Count).ToList();
                var join = (from s in source
                            join t in target on s equals t.Meta
                            select t).OrderBy(x => x.Position).ToList();

                if (join.Count == source.Count)
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

        private static bool CheckToken(DmIntentExpressionItem compositedEntity, List<DmIntentExpressionItem> unitEntities, int idx)
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
        private static List<DmIntentExpressionItem> Process(string text, List<DmIntentExpressionItem> entities)
        {
            var tags = new List<DmIntentExpressionItem>();

            entities.ForEach(token =>
            {
                MatchCollection mc = Regex.Matches(text, token.Text);
                foreach (Match m in mc)
                {
                    DmIntentExpressionItem temp = new DmIntentExpressionItem()
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

            var results = new List<DmIntentExpressionItem>();

            // 扫描字符串
            for (int pos = 0; pos < text.Length;)
            {
                // 查找实体
                var tag = tags.FirstOrDefault(x => x.Position == pos);
                if (tag != null)
                {
                    results.Add(tag);
                    pos += tag.Length;
                }
                else
                {
                    // 取下一个, 如果没找到实体，就一直取到最后一个字符。
                    tag = tags.FirstOrDefault(x => x.Position > pos);
                    int length = tag == null ? text.Length - pos : tag.Position - pos;

                    // 如果没有识别为实体，则切开每个字符。
                    // int length = 1;

                    var item = new DmIntentExpressionItem
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
