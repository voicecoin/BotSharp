using BotSharp.Core.Agents;
using BotSharp.Core.Engines;
using BotSharp.Core.Entities;
using BotSharp.Core.Intents;
using DotNetToolkit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voicebot.Core.Chatbots;
using Voicebot.Core.NlpService;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Intent controller
    /// </summary>
    public class IntentsController : CoreController
    {
        /// <summary>
        /// Markup sentence with entity
        /// </summary>
        /// <param name="lang">language code</param>
        /// <param name="text">user say</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("markup")]
        public VmIntentExpression Markup([FromQuery] string lang, [FromQuery] string text)
        {
            var expression = new VmIntentExpression();

            var builtInEntityTypes = dc.Table<EntityType>()
                .Where(x => x.AgentId == AiBot.BUILTIN_ZH_BOT_ID)
                .Select(x => x.Name)
                .ToList();

            INlpEngine nlp = new NlpIR();
            var entities = nlp.Ner(text);
            entities = entities.Where(x => builtInEntityTypes.Contains(x.Name)).ToList();

            if (entities.Count == 0)
            {
                expression.Data = new List<VmIntentExpressionPart>()
                {
                    new VmIntentExpressionPart { Text = text }
                };
            }
            else
            {
                expression.Data = FillEntities(text, entities);
            }

            return expression;
        }


        private List<VmIntentExpressionPart> FillEntities(string text, List<NlpEntity> entities)
        {
            int cursor = 0;

            var data = new List<VmIntentExpressionPart>();

            while (text.Length > 0)
            {
                entities.ForEach(nlpEntity =>
                {
                    int index = text.IndexOf(nlpEntity.Value);

                    if (index < 0)
                    {
                        data.Add(new VmIntentExpressionPart
                        {
                            Text = nlpEntity.Value,
                            Start = cursor
                        });

                        text = String.Empty;
                    }
                    else if (index == 0)
                    {
                        data.Add(new VmIntentExpressionPart
                        {
                            Meta = $"@sys.{nlpEntity.Name}",
                            Alias = nlpEntity.Name,
                            Text = nlpEntity.Value,
                            UserDefined = true,
                            Start = cursor
                        });

                        text = text.Substring(index + nlpEntity.Value.Length);

                        cursor += nlpEntity.Value.Length;
                    }
                    else if (index > 0)
                    {
                        var pre = text.Substring(0, index);

                        data.Add(new VmIntentExpressionPart
                        {
                            Text = pre,
                            Start = cursor
                        });
                        cursor += index;

                        data.Add(new VmIntentExpressionPart
                        {
                            Meta = $"@sys.{nlpEntity.Name}",
                            Alias = nlpEntity.Name,
                            Text = nlpEntity.Value,
                            UserDefined = true,
                            Start = cursor
                        });

                        text = text.Substring(index + nlpEntity.Value.Length);
                        cursor += nlpEntity.Value.Length;
                    }
                });
            }

            return data;
        }

        /// <summary>
        /// Create a intent for agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="intent"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateIntent([FromRoute] string agentId, [FromBody] VmIntentDetail intent)
        {
            var agent = new RasaAi(dc).LoadAgentById(dc, agentId);
            return agent.CreateIntent(dc, intent.ToObject<Intent>());
        }

        /// <summary>
        /// List all intents for specific agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        [HttpGet("{agentId}")]
        public PageResult<VmIntent> MyIntents([FromRoute] string agentId)
        {
            var result = new PageResult<VmIntent>() { };
            var query = dc.Table<Intent>().Where(x => x.AgentId == agentId).Select(x => x.ToObject<VmIntent>());
            return result.LoadRecords<VmIntent>(query);
        }

        /// <summary>
        /// Delete intent
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="intentId"></param>
        [HttpDelete("{agentId}/{intentId}")]
        public void DeleteIntent([FromRoute] string agentId, [FromRoute] string intentId)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intentId"></param>
        /// <param name="intent"></param>
        [HttpPut("{intentId}")]
        public void UpdateIntent([FromRoute] string intentId, [FromBody] VmIntentDetail intent)
        {

        }
    }
}
