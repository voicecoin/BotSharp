using Core;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Apps.Chatbot.Faq
{
    public class FaqController : CoreController
    {
        [HttpGet("{agentId}/Query")]
        public DmPageResult<FaqEntity> GetEntities(string agentId, [FromQuery] string name, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var query = dc.Table<FaqEntity>().Where(x => x.AgentId == agentId);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Question.Contains(name));
            }

            var total = query.Count();

            var items = query.Skip((page - 1) * size).Take(size).ToList();

            return new DmPageResult<FaqEntity> { Total = total, Page = page, Size = size, Items = items };
        }

        [HttpDelete("{agentId}/{id}")]
        public void Delete(string agentId, string id)
        {
            dc.Transaction<IDbRecord4Core>(delegate {
                var faq = dc.Table<FaqEntity>().Find(id);
                dc.Table<FaqEntity>().Remove(faq);
            });
        }

        [HttpPost]
        public void AddFaq([FromBody] FaqEntity entity)
        {
            if (entity.IsExist(dc)) return;

            dc.CurrentUser = GetCurrentUser();

            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedUserId = dc.CurrentUser.Id;
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedUserId = dc.CurrentUser.Id;

            dc.Transaction<IDbRecord4Core>(delegate
            {
                dc.Table<FaqEntity>().Add(entity);
            });
        }

        [AllowAnonymous]
        [HttpPost("upload/{agentId}")]
        public void UploadFaqAsync(IFormFile file, [FromRoute] string agentId)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads
            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var gb2312 = Encoding.GetEncoding("GB2312");
            var content = System.IO.File.ReadAllText(filePath, gb2312);
            var faqs = content.Split(new string[] { "-break-" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !String.IsNullOrEmpty(x)).ToList();

            dc.CurrentUser = GetCurrentUser();

            dc.Transaction<IDbRecord4Core>(delegate
            {
                faqs.ForEach(pair =>
                {
                    var faq = pair.Split('\r', '\n').Where(x => !String.IsNullOrEmpty(x)).ToArray();

                    FaqEntity entity = new FaqEntity
                    {
                        CreatedDate = DateTime.UtcNow,
                        CreatedUserId = dc.CurrentUser.Id,
                        ModifiedDate = DateTime.UtcNow,
                        ModifiedUserId = dc.CurrentUser.Id,
                        Question = faq[0],
                        Answer = faq[1],
                        AgentId = agentId
                    };

                    if (!entity.IsExist(dc))
                    {
                        dc.Table<FaqEntity>().Add(entity);
                    }
                    
                });
            });
        }

        [HttpGet("train/{agentId}")]
        public void Train(string agentId)
        {
            var corpus = dc.Table<FaqEntity>().Where(x => x.AgentId == agentId).Select(x => new { Label = x.Id, Corpus = x.Question }).ToList();

            List<String> list = new List<string>();
            for (int i = 0; i < corpus.Count; i++)
            {
                char[] chars = corpus[i].Corpus.ToCharArray();
                string line = $"__label__{corpus[i].Label} {String.Join(" ", chars)}";
                list.Add(line);
            }

            var obj = RestHelper.PostSync<String>("http://ai.yaya.ai:8004/FastTextReceive", new { name = agentId, corpus = list });
        }
    }
}
