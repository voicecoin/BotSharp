using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Core.Interfaces;
using Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using Apps.Chatbot.Agent;

namespace Apps.Chatbot.Faq
{
    public class Hooks : IHookDbInitializer
    {
        public int Priority => 1000;

        public void Load(IHostingEnvironment env, IConfiguration config, CoreDbContext dc)
        {
            var agentNames = LoadJson<List<String>>(env, "Agents");

            List<AgentEntity> agents = new List<AgentEntity>();

            agentNames.ForEach(agentName =>
            {
                var agent = LoadJson<AgentEntity>(env, $"{agentName}\\Agent");

                agents.Add(agent);
            });

            agents.ForEach(agent =>
            {
                InitFaqs(env, dc, agent);
            });
        }

        private static void InitFaqs(IHostingEnvironment env, CoreDbContext dc, AgentEntity agent)
        {
            var faqs = LoadJson<List<FaqEntity>>(env, $"{agent.Name}\\Faq");

            faqs?.ForEach(faq =>
            {
                var dm = new DomainModel<FaqEntity>(dc, new FaqEntity
                {
                    AgentId = agent.Id,
                    Question = faq.Question,
                    Answer = faq.Answer
                });
                dm.AddEntity();
            });
        }

        private static T LoadJson<T>(IHostingEnvironment env, string fileName)
        {
            string file = $"{env.ContentRootPath}\\App_Data\\" + fileName + ".json";
            if (!File.Exists(file))
            {
                return default(T);
            }

            string json;
            using (StreamReader SourceReader = File.OpenText(file))
            {
                json = SourceReader.ReadToEnd();
                if (String.IsNullOrEmpty(json))
                {
                    return default(T);
                }
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
