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
using Core.DomainModels;
using Core.Bundle;

namespace Apps.Chatbot.Agent
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 100;

        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            var dm = new DomainModel<BundleEntity>(dc, new BundleEntity { Name = "Chatbot Agent", EntityName = "Agent" });
            dm.AddEntity();

            InitAgent(env, dc);
        }

        private static void InitAgent(IHostingEnvironment env, CoreDbContext context)
        {
            // create a agent
            var agentNames = LoadJson<List<String>>(env, "Agents");

            string id = Guid.NewGuid().ToString();
            string id2 = Guid.NewGuid().ToString();
            string token1 = Guid.NewGuid().ToString("N");
            string token2 = Guid.NewGuid().ToString("N");

            List<AgentEntity> agents = new List<AgentEntity>();

            agentNames.ForEach(agentName =>
            {
                var agent = LoadJson<AgentEntity>(env, $"{agentName}\\Agent");

                BundleDomainModel<AgentEntity> dm = new BundleDomainModel<AgentEntity>(context, agent);
                dm.AddEntity();

                agents.Add(agent);
            });
        }

        private static DmEntity LoadEntityFromJsonFile(IHostingEnvironment env, AgentEntity agent, string name)
        {
            string json;
            using (StreamReader SourceReader = File.OpenText($"{env.ContentRootPath}\\App_Data\\{agent.Name}\\Entities\\{name}.json"))
            {
                json = SourceReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<DmEntity>(json);
        }

        private static T LoadJson<T>(IHostingEnvironment env, string fileName)
        {
            string json;
            using (StreamReader SourceReader = File.OpenText($"{env.ContentRootPath}\\App_Data\\" + fileName + ".json"))
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
