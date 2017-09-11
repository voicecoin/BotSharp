using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Apps.Chatbot.Faq
{
    public class FaqController : CoreController
    {
        [HttpGet("{agentId}/Query")]
        public DmPageResult<FaqEntity> GetEntities(string agentId, [FromQuery] string name, [FromQuery] int page = 1, [FromQuery] int size = 20)
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

        [HttpGet("train/{agentId}")]
        public void Train(string agentId)
        {
            var corpus = dc.Table<FaqEntity>().Where(x => x.AgentId == agentId).Select(x => new { Label = x.Id, Corpus = x.Question }).ToList();

            List<String> list = new List<string>();
            for (int i = 0; i < corpus.Count; i++)
            {
                char[] chars = corpus[i].Corpus.ToCharArray();
                string line = $"__{corpus[i].Label}__ {String.Join(" ", chars)}";
                list.Add(line);
            }

            var obj = RestHelper.PostSync<String>("http://ai.yaya.ai:8004/FastTextReceive", new { name = agentId, corpus = list });
        }
    }
}
