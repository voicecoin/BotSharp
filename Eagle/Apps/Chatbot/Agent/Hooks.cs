using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Newtonsoft.Json;
using Core.Interfaces;
using Apps.Chatbot.DomainModels;
using Apps.Chatbot.DmServices;
using Core;
using Apps.Chatbot.Intent;
using Apps.Chatbot.Entity;
using Core.Bundle;
using Microsoft.Extensions.Configuration;

namespace Apps.Chatbot.Agent
{
    public class Hooks : IHookDbInitializer
    {
        public int Priority => 100;

        public void Load(IHostingEnvironment env, IConfigurationRoot config, CoreDbContext dc)
        {
            var dm = new DomainModel<BundleEntity>(dc, new BundleEntity { Name = "Chatbot Agent", EntityName = "Agent" });
            dm.AddEntity();

            InitAgent(env, dc);

            InitAlignements(env, dc);
        }

        private static void InitAgent(IHostingEnvironment env, CoreDbContext context)
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
                BundleDomainModel<AgentEntity> dm = new BundleDomainModel<AgentEntity>(context, agent);
                dm.AddEntity();
            });
        }

        private static void InitAlignements(IHostingEnvironment env, CoreDbContext context)
        {
            var dm = new DomainModel<AgentSkillEntity>(context, new AgentSkillEntity
            {
                AgentId = "6dfd6dc6-2d63-408a-89cf-ee8ccef24c79",
                SkillId = "1cfb40a9-c26d-4ffd-9a05-186d33ea36e9"
            });

            dm.AddEntity();
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
