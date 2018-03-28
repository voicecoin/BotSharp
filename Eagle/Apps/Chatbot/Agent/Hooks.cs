using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Newtonsoft.Json;
using Core.Interfaces;
using Apps.Chatbot.Entity;
using Microsoft.Extensions.Configuration;
using EntityFrameworkCore.BootKit;

namespace Apps.Chatbot.Agent
{
    public class Hooks : IHookDbInitializer
    {
        public int Priority => 100;

        public void Load(IHostingEnvironment env, IConfiguration config, Database dc)
        {
            InitAgent(env, dc);
        }

        private static void InitAgent(IHostingEnvironment env, Database context)
        {
            // create a agent
            var agentNames = LoadJson<List<String>>(env, "Agents");

            if (agentNames == null) return;

            /*string id = Guid.NewGuid().ToString();
            string id2 = Guid.NewGuid().ToString();
            string token1 = Guid.NewGuid().ToString("N");
            string token2 = Guid.NewGuid().ToString("N");*/

            agentNames.ForEach(agentName =>
            {
                var agent = LoadJson<AgentEntity>(env, $"{agentName}\\Agent");
                /*BundleDomainModel<AgentEntity> dm = new BundleDomainModel<AgentEntity>(context, agent);
                dm.AddEntity();

                InitSkills(context, agent.Id, agent.Skills);*/
            });
        }

        private static void InitSkills(Database context, string agentId, List<string> skills)
        {
            if (skills == null) return;

            skills.ForEach(skill => {
                /*var dm = new DomainModel<AgentSkillEntity>(context, new AgentSkillEntity
                {
                    AgentId = agentId,
                    SkillId = skill
                });

                dm.AddEntity();*/
            });
        }

        private static EntityEntity LoadEntityFromJsonFile(IHostingEnvironment env, AgentEntity agent, string name)
        {
            string json;
            using (StreamReader SourceReader = File.OpenText($"{env.ContentRootPath}\\App_Data\\{agent.Name}\\Entities\\{name}.json"))
            {
                json = SourceReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<EntityEntity>(json);
        }

        private static T LoadJson<T>(IHostingEnvironment env, string fileName)
        {
            string filePath = $"{env.ContentRootPath}\\App_Data\\" + fileName + ".json";

            if (!File.Exists(filePath)) return default(T);

            string json;
            using (StreamReader SourceReader = File.OpenText(filePath))
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
