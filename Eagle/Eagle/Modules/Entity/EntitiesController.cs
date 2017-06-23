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
using AutoMapper;

namespace Eagle.Modules.Entity
{
    [Route("v1/Entities")]
    public class EntitiesController : ControllerBase
    {
        private readonly DataContexts _context = new DataContexts();

        // GET: api/Entities
        [HttpGet]
        public IEnumerable<Entities> GetEntities()
        {
            return _context.Entities.Take(10);
        }

        // GET: v1/Entities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntities([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entities = await _context.Entities.SingleOrDefaultAsync(m => m.Id == id);

            if (entities == null)
            {
                return NotFound();
            }

            var items = (from entry in _context.EntityEntries
                         join synonym in _context.EntityEntrySynonyms on entry.Id equals synonym.EntityEntryId
                         where entry.EntityId == id
                         select new { entry, synonym }).ToList();

            var entity = _context.Entities.Where(x => x.Id == id).Select(x => Mapper.Map<EntityModel>(x)).ToList();
            

            return Ok(entities);
        }

        // PUT: api/Entities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntities([FromRoute] string id, [FromBody] Entities entities)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entities.Id)
            {
                return BadRequest();
            }

            _context.Entry(entities).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntitiesExists(id))
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

        // POST: v1/Entities
        [HttpPost]
        public async Task<IActionResult> PostEntities([FromBody] Entities entities)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entities.Add(entities);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntities", new { id = entities.Id }, new { id = entities.Id });
        }

        // DELETE: api/Entities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntities([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entities = await _context.Entities.SingleOrDefaultAsync(m => m.Id == id);
            if (entities == null)
            {
                return NotFound();
            }

            _context.Entities.Remove(entities);
            await _context.SaveChangesAsync();

            return Ok(entities);
        }

        private bool EntitiesExists(string id)
        {
            return _context.Entities.Any(e => e.Id == id);
        }
    }
}