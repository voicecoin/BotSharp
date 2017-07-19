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

namespace Apps.Chatbot
{
    public class IntentsController : CoreController
    {
        // GET: v1/Intents
        [HttpGet("{agentId}/Query")]
        public DmPageResult<DmIntent> GetIntents(string agentId, [FromQuery] string name, [FromQuery] int page = 1)
        {
            var query = dc.Chatbot_Intents.Where(x => x.AgentId == agentId);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var total = query.Count();

            int size = 20;

            var items = query.Skip((page - 1) * size).Take(size).Select(x => x.Map<DmIntent>()).ToList();
            return new DmPageResult<DmIntent> { Total = total, Page = page, Size = size, Items = items };
        }

        // GET: v1/Intents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIntents([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var intents = await dc.Chatbot_Intents.SingleOrDefaultAsync(m => m.Id == id);

            if (intents == null)
            {
                return NotFound();
            }

            var intentModel = intents.Map<DmIntent>();
            intentModel.Load(dc);

            return Ok(intentModel);
        }

        // PUT: api/Intents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntents([FromRoute] string id, [FromBody] DmIntent intentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != intentModel.Id)
            {
                return BadRequest("Id is not match");
            }

            dc.Transaction(delegate
            {
                intentModel.Update(dc);
            });

            return Ok(id);
        }

        // POST: api/Intents
        [HttpPost("{agentId}")]
        public async Task<IActionResult> PostIntents(string agentId, [FromBody] DmIntent intentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dc.Transaction(delegate
            {
                intentModel.AgentId = agentId;
                intentModel.Add(dc);
            });

            return CreatedAtAction("GetIntents", new { id = intentModel.Id }, intentModel.Id);
        }

        // DELETE: api/Intents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntents([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var intents = await dc.Chatbot_Intents.SingleOrDefaultAsync(m => m.Id == id);
            if (intents == null)
            {
                return NotFound();
            }

            dc.Chatbot_Intents.Remove(intents);
            await dc.SaveChangesAsync();

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
                x.Meta,
                x.Position,
                x.Length,
                Color = x.Color
            }).OrderBy(x => x.Position);
        }

        private bool IntentsExists(string id)
        {
            return dc.Chatbot_Intents.Any(e => e.Id == id);
        }
    }
}