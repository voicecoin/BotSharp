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
using Apps.Chatbot.Agent;

namespace Apps.Chatbot.Entity
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 110;

        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            if (!dc.Table<BundleEntity>().Any(x => x.EntityName == "Agent"))
            {
                DmBundle bundle = new DmBundle { Name = "Chatbot Agent", EntityName = "Agent" };
                bundle.Add(dc);
                dc.SaveChanges();
            }

            var agentNames = LoadJson<List<String>>(env, "Agents");

            List<AgentEntity> agents = new List<AgentEntity>();

            agentNames.ForEach(agentName =>
            {
                var agent = LoadJson<AgentEntity>(env, $"{agentName}\\Agent");

                BundleDomainModel<AgentEntity> dm = new BundleDomainModel<AgentEntity>(dc, agent);
                dm.AddEntity();

                agents.Add(agent);
            });

            agents.ForEach(agent =>
            {
                InitEntities(env, dc, agent);
            });
        }

        private static void InitEntities(IHostingEnvironment env, CoreDbContext context, AgentEntity agent)
        {
            var entityNames = Directory.GetFiles($"{env.ContentRootPath}\\App_Data\\{agent.Name}\\Entities").Select(x => x.Split('\\').Last().Split('.').First()).ToList();

            entityNames.ForEach(entityName =>
            {
                if (context.Table<EntityEntity>().Count(x => x.Name == entityName) == 0)
                {
                    // add entity
                    DmEntity entity = LoadEntityFromJsonFile(env, agent, entityName);
                    entity.AgentId = agent.Id;
                    entity.Name = entityName;
                    entity.Add(context);
                }
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
