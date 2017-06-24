using AutoMapper;
using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.Enums;
using Eagle.Models;
using Eagle.Modules.Analyzer;
using Eagle.Utility;
using Microsoft.AspNetCore.Hosting;
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
        public static void Initialize(IHostingEnvironment env, DataContexts context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Agents.Any())
            {
                return;   // DB has been seeded
            }

            Agents agent = InitAgent(env, context);
            InitEntities(env, context, agent.Id);
            InitIntents(env, context, agent);

            context.SaveChanges();
        }

        private static Agents InitAgent(IHostingEnvironment env, DataContexts context)
        {
            // create a agent
            var agent = new Agents()
            {
                Id = Constants.GenesisAgentId,
                UserId = Constants.GenesisUserId,
                ClientAccessToken = Constants.GenesisAccessToken,
                DeveloperAccessToken = Constants.GenesisDeveloperToken,
                Name = "小丫",
                Description = "人工智能通用机器人。"
            };

            context.Agents.Add(agent);

            return agent;
        }

        private static void InitIntents(IHostingEnvironment env, DataContexts context, Agents agent)
        {
            var intent = new Intents()
            {
                Id = "ee560fa6-1a28-43be-a027-7e2f523b3a00",
                AgentId = agent.Id,
                Name = "问带时间和地点的天气情况"
            };

            context.Intents.Add(intent);

            var intentExpression = new IntentExpressions()
            {
                IntentId = intent.Id
            };
            context.IntentExpressions.Add(intentExpression);

            context.SaveChanges();

            // add user say
            /*new NerController().Ner("翌日京城天气如何").ForEach(itemModel =>
            {
                var itemRecord = Mapper.Map<IntentExpressionItems>(itemModel);
                itemRecord.IntentExpressionId = intentExpression.Id;
                itemRecord.CreatedUserId = Constants.GenesisUserId;
                context.IntentExpressionItems.Add(itemRecord);
            });*/
        }

        private static void InitEntities(IHostingEnvironment env, DataContexts context, string agentId)
        {
            var entityNames = Directory.GetFiles($"{env.ContentRootPath}\\App_Data\\Entity").Select(x => x.Split('\\').Last().Split('.').First()).ToList();

            entityNames.ForEach(entityName => {

                // add entity
                EntityModel entity = LoadEntityFromJsonFile(env, entityName);

                var entityRecord = new Entities()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = entityName,
                    AgentId = agentId,
                    IsEnum = entity.IsEnum,
                    IsOverridable = entity.IsOverridable,
                };

                context.Entities.Add(entityRecord);

                // add entries
                entity.Entries.Where(x => !String.IsNullOrEmpty(x.Value)).ToList().ForEach(entry =>
                {
                    var entryRecord = new EntityEntries { EntityId = entityRecord.Id, Value = entry.Value };
                    /*if (String.IsNullOrEmpty(entry.Template) && !entity.IsEnum)
                    {
                        entryRecord.Template = $"@{entityName}";
                    }
                    else
                    {
                        entryRecord.Template = entry.Template;
                    }*/

                    context.EntityEntries.Add(entryRecord);

                    // add synonyms
                    if (entry.Synonyms != null)
                    {
                        entry.Synonyms.Where(x => !String.IsNullOrEmpty(x)).ToList().ForEach(synonym => {

                            context.EntityEntrySynonyms.Add(new EntityEntrySynonyms
                            {
                                EntityEntryId = entryRecord.Id,
                                Synonym = synonym
                            });

                        });
                    }

                });

            });
        }

        private static EntityModel LoadEntityFromJsonFile(IHostingEnvironment env, string name)
        {
            string json;
            using (StreamReader SourceReader = File.OpenText($"{env.ContentRootPath}\\App_Data\\Entity\\{name}.json"))
            {
                json = SourceReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<EntityModel>(json);
        }
    }
}
