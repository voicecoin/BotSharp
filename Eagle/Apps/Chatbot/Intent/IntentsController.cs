using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core;
using Apps.Chatbot.DomainModels;
using Utility;
using Apps.Chatbot.DmServices;
using Apps.Chatbot.Intent;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using EntityFrameworkCore.BootKit;

namespace Apps.Chatbot.Intent
{
    public class IntentsController : CoreController
    {
        // GET: v1/Intents
        [HttpGet("{agentId}/Query")]
        public DmPageResult<IntentEntity> GetIntents(string agentId, [FromQuery] string name, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var query = dc.Table<IntentEntity>().Where(x => x.AgentId == agentId);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var total = query.Count();

            var items = query.Skip((page - 1) * size).Take(size).Select(x => x.Map<IntentEntity>()).ToList();
            return new DmPageResult<IntentEntity> { Total = total, Page = page, Size = size, Items = items };
        }

        // GET: v1/Intents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIntents([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dm = new DomainModel<IntentEntity>(dc, new IntentEntity { Id = id });
            dm.Load();

            return Ok(dm.Entity);
        }

        [HttpGet]
        public async Task<IActionResult> InitIntent()
        {
            var intent = new IntentEntity()
            {
                Name = "未命名意图",
                Contexts = new List<string>(),
                Events = new List<string>(),
                UserSays = new List<IntentExpressionEntity>(),
                Responses = new List<IntentResponseEntity>()
                {
                    new IntentResponseEntity
                    {
                        AffectedContexts = new List<DmIntentResponseContext>(),
                        Parameters = new List<IntentResponseParameterEntity>(),
                        Messages = new List<IntentResponseMessageEntity>()
                    }
                }
            };

            return Ok(intent);
        }

        // PUT: api/Intents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntents([FromRoute] string id, [FromBody] IntentEntity intentModel)
        {
            if (id != intentModel.Id)
            {
                return BadRequest("Id is not match");
            }

            dc.Transaction<IDbRecord>(delegate
            {
                new DomainModel<IntentEntity>(dc, intentModel).Update();
            });

            return Ok(new { Id = intentModel.Id });
        }

        // POST: api/Intents
        [HttpPost("{agentId}")]
        public async Task<IActionResult> PostIntents(string agentId, [FromBody] IntentEntity intentEntity)
        {
            bool result = false;

            dc.Transaction<IDbRecord>(delegate
            {
                intentEntity.AgentId = agentId;
                result = new DomainModel<IntentEntity>(dc, intentEntity).AddEntity();
            });

            if (result)
            {
                return Ok(intentEntity);
            }
            else
            {
                return BadRequest("创建失败, 请检查配置。");
            }
        }

        // DELETE: api/Intents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntents([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var intents = await dc.Table<IntentEntity>().SingleOrDefaultAsync(m => m.Id == id);
            if (intents == null)
            {
                return NotFound();
            }

            dc.Table<IntentEntity>().Remove(intents);
            dc.SaveChanges();

            return Ok(intents);
        }

        // GET: v1/Intents/Markup?text=
        [HttpGet("Markup")]
        public IntentExpressionEntity Markup([FromQuery] string text)
        {
            var model = new DmAgentRequest { Text = text };

            var data = model.PosTagger(dc).Select(x => new DmIntentExpressionItem
            {
                Text = x.Text,
                Alias = x.Alias,
                Meta = x.Meta,
                Position = x.Position,
                Length = x.Length,
                Color = x.Color,
                Value = x.Value
            }).OrderBy(x => x.Position).ToList();

            var userSay = new IntentExpressionEntity() { Text = text, Data = data};

            return userSay;
        }

        private bool IntentsExists(string id)
        {
            return dc.Table<IntentEntity>().Any(e => e.Id == id);
        }
    }
}