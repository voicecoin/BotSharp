using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eagle.DbContexts;
using Eagle.Models;
using Eagle.Utility;
using Eagle.DddServices;

namespace Eagle.Modules.Entity
{
    [Route("v1/EntityEntries")]
    public class EntityEntriesController : ControllerBase
    {
        private readonly DataContexts _context = new DataContexts();

        // GET: v1/EntityEntries
        [HttpGet("{entityId}/Query")]
        public PageResultModel<EntityEntryModel> GetEntityEntries(string entityId, string name, [FromQuery] int page = 1)
        {
            var query = _context.EntityEntries.Where(x => x.EntityId == entityId);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Value.Contains(name));
            }

            var total = query.Count();

            int size = 20;

            var items = query.Skip((page - 1) * size).Take(size).Select(x => x.Map<EntityEntryModel>()).ToList();
            return new PageResultModel<EntityEntryModel> { Total = total, Page = page, Size = size, Items = items };
        }

        // GET: v1/EntityEntries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntityEntries([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityEntries = await _context.EntityEntries.SingleOrDefaultAsync(m => m.Id == id);

            if (entityEntries == null)
            {
                return NotFound();
            }

            return Ok(entityEntries);
        }

        // PUT: api/EntityEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntityEntries([FromRoute] string id, [FromBody] EntityEntryModel entityEntryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entityEntryModel.Id)
            {
                return BadRequest();
            }

            _context.Transaction(delegate {
                entityEntryModel.Update(_context);
            });

            return Ok(entityEntryModel.Id);
        }

        // POST: api/EntityEntries
        [HttpPost("{entityId}")]
        public async Task<IActionResult> PostEntityEntry(string entityId, [FromBody] EntityEntryModel entityEntryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Transaction(delegate {
                entityEntryModel.EntityId = entityId;
                entityEntryModel.Add(_context);
            });

            return CreatedAtAction("GetEntityEntries", new { id = entityEntryModel.Id }, entityEntryModel.Id);
        }

        // DELETE: api/EntityEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntityEntries([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityEntries = await _context.EntityEntries.SingleOrDefaultAsync(m => m.Id == id);
            if (entityEntries == null)
            {
                return NotFound();
            }

            _context.Transaction(delegate {
                entityEntries.Map<EntityEntryModel>().Delete(_context);
            });

            return Ok(entityEntries.Id);
        }

        private bool EntityEntriesExists(string id)
        {
            return _context.EntityEntries.Any(e => e.Id == id);
        }
    }
}