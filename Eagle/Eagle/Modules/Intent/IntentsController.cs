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
using Eagle.Utility;
using Eagle.DddServices;

namespace Eagle.Modules.Intent
{
    [Route("v1/Intents")]
    public class IntentsController : ControllerBase
    {
        private readonly DataContexts _context = new DataContexts();

        // GET: v1/Intents
        [HttpGet("{agentId}/Query")]
        public PageResultModel<IntentModel> GetIntents(string agentId, [FromQuery] string name, [FromQuery] int page = 1)
        {
            var query = _context.Intents.Where(x => x.AgentId == agentId);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var total = query.Count();

            int size = 20;

            var items = query.Skip((page - 1) * size).Take(size).Select(x => x.Map<IntentModel>()).ToList();
            return new PageResultModel<IntentModel> { Total = total, Page = page, Size = size, Items = items };
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
        public async Task<IActionResult> PutIntents([FromRoute] string id, [FromBody] IntentModel intentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != intentModel.Id)
            {
                return BadRequest();
            }

            _context.Transaction(delegate
            {
                intentModel.Update(_context);
            });

            return NoContent();
        }

        // POST: api/Intents
        [HttpPost("{agentId}")]
        public async Task<IActionResult> PostIntents(string agentId, [FromBody] IntentModel intentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Transaction(delegate
            {
                intentModel.AgentId = agentId;
                intentModel.Add(_context);
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

            var intents = await _context.Intents.SingleOrDefaultAsync(m => m.Id == id);
            if (intents == null)
            {
                return NotFound();
            }

            _context.Intents.Remove(intents);
            await _context.SaveChangesAsync();

            return Ok(intents);
        }

        // GET: v1/Intent?text=
        [HttpGet("Markup")]
        public IEnumerable<Object> Markup([FromQuery] string text)
        {
            var model = new AnalyzerModel { Text = text };

            return model.Ner(_context).Select(x => new
            {
                x.Text,
                x.Alias,
                x.Meta,
                x.Position,
                x.Length,
                x.Unit,
                x.EntityId
            }).OrderBy(x => x.Unit);
        }

        private bool IntentsExists(string id)
        {
            return _context.Intents.Any(e => e.Id == id);
        }
    }
}