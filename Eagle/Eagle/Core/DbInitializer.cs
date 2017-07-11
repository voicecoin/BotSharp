using Eagle.Apps.Chatbot.DmServices;
using Eagle.Apps.Chatbot.DomainModels;
using Eagle.DataContexts;
using Eagle.DbTables;
using Eagle.DmServices;
using Eagle.DomainModels;
using Eagle.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Eagle.Core
{
    public class DbInitializer
    {
        public static void Initialize(IHostingEnvironment env)
        {
            CoreDbContext context = new CoreDbContext(new DbContextOptions<CoreDbContext>());
            //var dbContexts = serviceProvider.GetService<DataContexts>();

            context.Database.EnsureCreated();

            InitAgent(env, context);
        }

        private static void InitAgent(IHostingEnvironment env, CoreDbContext context)
        {
            // create a agent
            var agentNames = LoadJson<List<String>>(env, "Agents");

            string id = Guid.NewGuid().ToString();
            string id2 = Guid.NewGuid().ToString();
            string token1 = Guid.NewGuid().ToString("N");
            string token2 = Guid.NewGuid().ToString("N");

            List<Agents> agents = new List<Agents>();

            agentNames.ForEach(agentName => {

                var agent = LoadJson<Agents>(env, $"{agentName}\\Agent");

                if (!context.Agents.Any(x => x.Name == agentName))
                {
                    context.Transaction(delegate
                    {
                        context.Agents.Add(agent);
                    });
                }

                agents.Add(agent);
            });

            agents.ForEach(agent =>
            {
                InitEntities(env, context, agent);
            });

            agents.ForEach(agent =>
            {
                InitIntents(env, context, agent);
            });
        }

        private static void InitIntents(IHostingEnvironment env, CoreDbContext context, Agents agent)
        {
            var intentNames = Directory.GetFiles($"{env.ContentRootPath}\\App_Data\\{agent.Name}\\Intents").Select(x => x.Split('\\').Last().Split('.').First()).ToList();

            intentNames.ForEach(intentName =>
            {
                if (!context.Intents.Any(x => x.AgentId == agent.Id && x.Name.Equals(intentName)))
                {
                    // Intent
                    var intentModel = LoadJson<DmIntent>(env, $"{agent.Name}\\Intents\\{intentName}");
                    intentModel.AgentId = agent.Id;
                    intentModel.Name = intentName;

                    context.Transaction(delegate
                    {
                        intentModel.Add(context);
                    });
                }
            });

            /*var intentRecord = intentModel.Map<Intents>();
            context.Intents.Add(intentRecord);

            // User expression
            intentModel.UserSays.ForEach(expression =>
            {

                expression.Id = Guid.NewGuid().ToString();
                expression.IntentId = intentRecord.Id;

                // Markup
                var model = new DmAgentRequest { Text = expression.Text };
                model.PosTagger(context).ForEach(itemModel =>
                {
                    itemModel.IntentExpressionId = expression.Id;
                    expression.Data.Add(itemModel);
                });

                expression.Add(context);
            });

            // Bot response
            intentModel.Responses.ForEach(response =>
            {
                response.IntentId = intentRecord.Id;
                response.Add(context);
            });*/
        }

        private static void InitEntities(IHostingEnvironment env, CoreDbContext context, Agents agent)
        {
            var entityNames = Directory.GetFiles($"{env.ContentRootPath}\\App_Data\\{agent.Name}\\Entities").Select(x => x.Split('\\').Last().Split('.').First()).ToList();

            entityNames.ForEach(entityName =>
            {
                context.Transaction(delegate
                {
                    if(context.Entities.Count(x => x.Name == entityName) == 0)
                    {
                        // add entity
                        DmEntity entity = LoadEntityFromJsonFile(env, agent, entityName);
                        entity.AgentId = agent.Id;
                        entity.Name = entityName;
                        entity.Add(context);
                    }
                });
            });
        }

        private static DmEntity LoadEntityFromJsonFile(IHostingEnvironment env, Agents agent, string name)
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
