using AutoMapper;
using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.Enums;
using Eagle.Model.Extensions;
using Eagle.Models;
using Eagle.Modules.Analyzer;
using Eagle.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle
{
    public class DbInitializer
    {
        public static void Initialize(IHostingEnvironment env)
        {
            DataContexts context = new DataContexts(new DbContextOptions<DataContexts>());
            //var dbContexts = serviceProvider.GetService<DataContexts>();

            context.Database.EnsureCreated();

            InitAgent(env, context);
        }

        private static void InitAgent(IHostingEnvironment env, DataContexts context)
        {
            // create a agent
            var agentNames = LoadJson<List<String>>(env, "Agents");

            string id = Guid.NewGuid().ToString();
            string id2 = Guid.NewGuid().ToString();
            string token1 = Guid.NewGuid().ToString("N");
            string token2 = Guid.NewGuid().ToString("N");

            List<Agents> agents = new List<Agents>();

            agentNames.ForEach(agentName => {

                if (context.Agents.Any(x => x.Name == agentName))
                {
                    return; 
                }

                var agent = LoadJson<Agents>(env, $"{agentName}\\Agent");

                context.Transaction(delegate
                {
                    context.Agents.Add(agent);

                    InitEntities(env, context, agent);
                });

                agents.Add(agent);

            });

            agents.ForEach(agent =>
            {
                context.Transaction(delegate
                {
                    InitIntents(env, context, agent);
                });
            });
        }

        private static void InitIntents(IHostingEnvironment env, DataContexts context, Agents agent)
        {
            var intent = new Intents()
            {
                AgentId = agent.Id,
                Name = "问带时间和地点的天气情况"
            };

            context.Intents.Add(intent);

            var intentExpression = new IntentExpressions()
            {
                IntentId = intent.Id,
                Text = "明天江西天气怎么样"
            };
            context.IntentExpressions.Add(intentExpression);

            // add user say
            var model = new AnalyzerModel { Text = intentExpression.Text };
            model.Ner(context).ForEach(itemModel =>
            {
                var itemRecord = itemModel.MapByJsonString<IntentExpressionItems>();
                itemRecord.IntentExpressionId = intentExpression.Id;
                itemRecord.CreatedDate = DateTime.UtcNow;
                context.IntentExpressionItems.Add(itemRecord);
            });
        }

        private static void InitEntities(IHostingEnvironment env, DataContexts context, Agents agent)
        {
            var entityNames = Directory.GetFiles($"{env.ContentRootPath}\\App_Data\\{agent.Name}\\Entities").Select(x => x.Split('\\').Last().Split('.').First()).ToList();

            entityNames.ForEach(entityName =>
            {
                // add entity
                EntityModel entity = LoadEntityFromJsonFile(env, agent, entityName);
                entity.AgentId = agent.Id;
                entity.Name = entityName;
                entity.Add(context);
            });
        }

        private static EntityModel LoadEntityFromJsonFile(IHostingEnvironment env, Agents agent, string name)
        {
            string json;
            using (StreamReader SourceReader = File.OpenText($"{env.ContentRootPath}\\App_Data\\{agent.Name}\\Entities\\{name}.json"))
            {
                json = SourceReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<EntityModel>(json);
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
