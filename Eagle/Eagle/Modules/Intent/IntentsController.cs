using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.Models;

namespace Eagle.Modules.Intent
{
    [Route("v1/Intents")]
    public class IntentsController : ControllerBase
    {
        private readonly DataContexts _context = new DataContexts();

        // GET: api/Intents
        [HttpGet]
        public IEnumerable<Intents> GetIntents()
        {
            return _context.Intents;
        }

        // GET: v1/Intents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIntents([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var intents = await _context.Intents.SingleOrDefaultAsync(m => m.Id == id);

            if (intents == null)
            {
                return NotFound();
            }

            var intentExpressions = _context.IntentExpressions.Where(x => x.IntentId == intents.Id).ToList();
            var intentExpressionItems = (from item in _context.IntentExpressionItems
                                         from entity in _context.Entities.Where(x => item.EntityId == x.Id).DefaultIfEmpty()
                                         where intentExpressions.Select(expression => expression.Id).Contains(item.IntentExpressionId)
                                         orderby item.Position
                                         select new { item, entity }).ToList();

            var intentModel = new IntentModel()
            {
                Id = intents.Id,
                Name = intents.Name,
                UserSays = intentExpressions.Select(expression => new IntentExpressionModel
                {
                    Id = expression.Id,
                    Data = intentExpressionItems.Where(item => item.item.IntentExpressionId == expression.Id)
                        .Select(item => new IntentExpressionItemModel
                        {
                            Text = item.item.Text,
                            Meta = item.entity == null ? null : $"@{item.entity?.Name}",
                            Alias = item.entity?.Name
                        }).ToList()
                }).ToList(),
                Templates = new List<string>()
            };

            intentModel.UserSays.ForEach(expression => {
                string template = String.Join(" ", expression.Data.Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : $"{x.Meta}:{x.Alias}").ToArray());
                intentModel.Templates.Add(template);
            });

            return Ok(intentModel);
        }

        // PUT: api/Intents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntents([FromRoute] string id, [FromBody] Intents intents)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != intents.Id)
            {
                return BadRequest();
            }

            _context.Entry(intents).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntentsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Intents
        [HttpPost]
        public async Task<IActionResult> PostIntents([FromBody] Intents intents)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Intents.Add(intents);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIntents", new { id = intents.Id }, intents);
        }

        // DELETE: api/Intents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntents([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var intents = await _context.Intents.SingleOrDefaultAsync(m => m.Id == id);
            if (intents == null)
            {
                return NotFound();
            }

            _context.Intents.Remove(intents);
            await _context.SaveChangesAsync();

            return Ok(intents);
        }

        private bool IntentsExists(string id)
        {
            return _context.Intents.Any(e => e.Id == id);
        }
    }
}