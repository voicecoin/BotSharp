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
using Enyim.Caching;

namespace Apps.Chatbot
{
    public class IntentsController : CoreController
    {
        public IntentsController(IMemcachedClient memcachedClient)
        {
            dc.MemcachedClient = memcachedClient;
        }

        // GET: v1/Intents
        [HttpGet("{agentId}/Query")]
        public DmPageResult<IntentEntity> GetIntents(string agentId, [FromQuery] string name, [FromQuery] int page = 1)
        {
            var query = dc.Table<IntentEntity>().Where(x => x.AgentId == agentId);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var total = query.Count();

            int size = 20;

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

        // PUT: api/Intents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntents([FromRoute] string id, IntentEntity intentModel)
        {
            if (id != intentModel.Id)
            {
                return BadRequest("Id is not match");
            }

            dc.Transaction<IDbRecord4SqlServer>(delegate
            {
                new DomainModel<IntentEntity>(dc, intentModel).Update();
            });

            return Ok(new { Id = intentModel.Id });
        }

        // POST: api/Intents
        [HttpPost("{agentId}")]
        public async Task<IActionResult> PostIntents(string agentId, IntentEntity intentModel)
        {
            dc.Transaction<IDbRecord4SqlServer>(delegate
            {
                intentModel.AgentId = agentId;
                new DomainModel<IntentEntity>(dc, intentModel).Add();
            });

            return Ok(new { Id = intentModel.Id });
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

        // GET: v1/Intent?text=
        [HttpGet("Markup")]
        public IEnumerable<object> Markup([FromQuery] string text)
        {
            var model = new DmAgentRequest { Text = text };

            return model.PosTagger(dc).Select(x => new
            {
                x.Text,
                x.Alias,
                DataType = x.Meta,
                x.Position,
                x.Length,
                Color = x.Color
            }).OrderBy(x => x.Position);
        }

        private bool IntentsExists(string id)
        {
            return dc.Table<IntentEntity>().Any(e => e.Id == id);
        }
    }
}