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
using Apps.Chatbot.Agent;
using Microsoft.Extensions.Configuration;
using EntityFrameworkCore.BootKit;

namespace Apps.Chatbot.Entity
{
    public class Hooks : IHookDbInitializer
    {
        public int Priority => 110;

        public void Load(IHostingEnvironment env, IConfiguration config, Database dc)
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
                InitEntities(env, dc, agent);
            });
        }

        private static void InitEntities(IHostingEnvironment env, Database context, AgentEntity agent)
        {
            string dir = $"{env.ContentRootPath}\\App_Data\\{agent.Name}\\Entities";
            if (!Directory.Exists(dir)) return;

            var entityNames = Directory.GetFiles(dir).Select(x => x.Split('\\').Last().Split('.').First()).ToList();

            entityNames.ForEach(entityName =>
            {
                if (context.Table<EntityEntity>().Count(x => x.Name == entityName) == 0)
                {
                    // add entity
                    EntityEntity entity = LoadEntityFromJsonFile(env, agent, entityName);
                    entity.AgentId = agent.Id;
                    entity.Name = entityName;
                    entity.Add(context);
                }
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
