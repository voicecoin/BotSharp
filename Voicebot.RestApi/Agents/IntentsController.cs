using BotSharp.Core.Agents;
using BotSharp.Core.Engines;
using BotSharp.Core.Entities;
using BotSharp.Core.Intents;
using DotNetToolkit;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [HttpPost("{agentId}")]
        public IActionResult CreateIntent([FromRoute] string agentId, [FromBody] VmIntentDetail vmIntent)
        {
            if(agentId != vmIntent.AgentId)
            {
                return BadRequest("AgentId is not matched with intent agentId.");
            }

            var agent = new RasaAi(dc).LoadAgentById(dc, agentId);
            var intent = vmIntent.ToIntent();

            dc.DbTran(() => {
                intent.Id = agent.CreateIntent(dc, intent);
            });

            return Ok(intent.Id);
        }

        /// <summary>
        /// List all intents for specific agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="page">page number</param>
        /// <returns></returns>
        [HttpGet("{agentId}/Query")]
        public PageResult<VmIntent> MyIntents([FromRoute] string agentId, [FromQuery] int page = 1)
        {
            var result = new PageResult<VmIntent>() { Page = page };
            var query = dc.Table<Intent>().Where(x => x.AgentId == agentId).Select(x => x.ToObject<VmIntent>());
            return result.LoadRecords<VmIntent>(query);
        }

        /// <summary>
        /// Get intent detail config
        /// </summary>
        /// <param name="intentId"></param>
        /// <returns></returns>
        [HttpGet("{intentId}")]
        public VmIntentDetail GetIntentById([FromRoute] string intentId)
        {
            var agent = new RasaAi(dc);
            var intent = agent.GetIntent(dc, intentId);
            var vm = new VmIntentDetail
            {
                Id = intent.Id,
                AgentId = intent.AgentId,
                Name = intent.Name,
                Events = new List<string>(),
                Contexts = intent.Contexts.Select(x => x.Name).ToList(),
                UserSays = intent.UserSays.Select(x => x.ToObject<VmIntentExpression>()).ToList(),
                Responses = intent.Responses.Select(x => {
                    var response = x.ToObject<VmIntentResponse>();
                    response.AffectedContexts = x.Contexts.Select(ctx => ctx.ToObject<VmIntentResponseContext>()).ToList();

                    response.Messages = x.Messages.Select(msg => {

                        if (msg.Speech == null) return new VmIntentResponseMessage();

                        return new VmIntentResponseMessage
                        {
                            Payload = msg.Payload,
                            Type = msg.Type,
                            Speeches = JsonConvert.DeserializeObject<List<String>>(msg.Speech)
                        };

                    }).ToList();

                    return response;
                } ).ToList()
            };
      
            return vm;
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
        public IActionResult UpdateIntent([FromRoute] string intentId, [FromBody] VmIntentDetail vmIntent)
        {
            var agent = new RasaAi(dc);

            dc.DbTran(() => {
                var intent = agent.GetIntent(dc, intentId);

                // remove
                var originalIntent = dc.Table<Intent>().Find(intentId);
                dc.Table<Intent>().Remove(originalIntent);
                dc.SaveChanges();

                // add back
                intent = vmIntent.ToIntent(intent);

                dc.Table<Intent>().Add(intent);
            });

            return Ok();
        }
    }
}
