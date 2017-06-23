using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eagle.DbContexts;
using Eagle.DbTables;

namespace Eagle.Modules.Entity
{
    [Route("v1/EntityEntries")]
    public class EntityEntriesController : ControllerBase
    {
        private readonly DataContexts _context = new DataContexts();

        // GET: v1/EntityEntries
        [HttpGet]
        public IEnumerable<EntityEntries> GetEntityEntries()
        {
            return _context.EntityEntries.Take(10);
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
        public async Task<IActionResult> PutEntityEntries([FromRoute] string id, [FromBody] EntityEntries entityEntries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entityEntries.Id)
            {
                return BadRequest();
            }

            _context.Entry(entityEntries).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityEntriesExists(id))
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

        // POST: api/EntityEntries
        [HttpPost]
        public async Task<IActionResult> PostEntityEntries([FromBody] EntityEntries entityEntries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EntityEntries.Add(entityEntries);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntityEntries", new { id = entityEntries.Id }, entityEntries);
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

            _context.EntityEntries.Remove(entityEntries);
            await _context.SaveChangesAsync();

            return Ok(entityEntries);
        }

        private bool EntityEntriesExists(string id)
        {
            return _context.EntityEntries.Any(e => e.Id == id);
        }
    }
}