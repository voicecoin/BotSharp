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

namespace Apps.Chatbot.Intent
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 120;

        public void Load(IHostingEnvironment env, CoreDbContext dc)
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
                InitIntents(env, dc, agent);
            });
        }

        private static void InitIntents(IHostingEnvironment env, CoreDbContext context, AgentEntity agent)
        {
            var intentNames = Directory.GetFiles($"{env.ContentRootPath}\\App_Data\\{agent.Name}\\Intents").Select(x => x.Split('\\').Last().Split('.').First()).ToList();

            intentNames.ForEach(intentName =>
            {
                // Intent
                var intentModel = LoadJson<IntentEntity>(env, $"{agent.Name}\\Intents\\{intentName}");
                intentModel.AgentId = agent.Id;
                intentModel.Name = intentName;

                new DomainModel<IntentEntity>(context, intentModel).Add();
            });
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
