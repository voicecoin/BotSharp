using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Core;
using Apps.Chatbot.DomainModels;
using Utility;
using Apps.Chatbot.DmServices;
using Core.Interfaces;
using Enyim.Caching;

namespace Apps.Chatbot.Entity
{
    public class EntitiesController : CoreController
    {
        // GET: api/Entities
        [HttpGet("{agentId}/Query")]
        public DmPageResult<DmEntity> GetEntities(string agentId, [FromQuery] string name, [FromQuery] int page = 1)
        {
            var query = dc.Table<EntityEntity>().Where(x => x.AgentId == agentId);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var total = query.Count();

            int size = 20;

            var items = query.Skip((page - 1) * size).Take(size).Select(x => x.Map<DmEntity>()).ToList();
            return new DmPageResult<DmEntity> { Total = total, Page = page, Size = size, Items = items };
        }

        // GET: v1/Entities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntity([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entities = await dc.Table<EntityEntity>().SingleOrDefaultAsync(m => m.Id == id);

            if (entities == null)
            {
                return NotFound();
            }

            var entity = dc.Table<EntityEntity>().Find(id).Map<DmEntity>();

            /*var items = (from entry in _context.EntityEntries
                         join synonym in _context.EntityEntrySynonyms on entry.Id equals synonym.EntityEntryId
                         where entry.EntityId == id
                         select new { synonym.EntityEntryId, synonym.Synonym, entry.Value, }).Take(100).ToList();

            entity.Entries = items.Select(x => new EntityEntryModel
            {
                Value = x.Value,
                Id = x.EntityEntryId,
                Synonyms = items.Where(syn => syn.EntityEntryId == x.EntityEntryId)
                .Select(syn => syn.Synonym)
                .ToList()
            });*/

            return Ok(entity);
        }

        // PUT: api/Entities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntities([FromRoute] string id, [FromBody] DmEntity entityModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entityModel.Id)
            {
                return BadRequest();
            }

            dc.Transaction<IDbRecord4SqlServer>(delegate {
                entityModel.Update(dc);
            });

            return Ok(entityModel.Id);
        }

        // POST: v1/Entities
        [HttpPost("{agentId}")]
        public async Task<IActionResult> PostEntities(string agentId, [FromBody] DmEntity entityModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dc.Transaction<IDbRecord4SqlServer>(delegate
            {
                entityModel.AgentId = agentId;
                entityModel.Add(dc);
            });
            

            return CreatedAtAction("GetEntities", new { id = entityModel.Id }, new { id = entityModel.Id });
        }

        // DELETE: api/Entities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntities([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entities = await dc.Table<EntityEntity>().SingleOrDefaultAsync(m => m.Id == id);
            if (entities == null)
            {
                return NotFound();
            }

            dc.Transaction<IDbRecord4SqlServer>(delegate {
                entities.Map<DmEntity>().Delete(dc);
            });

            return Ok(entities.Id);
        }

        private bool EntitiesExists(string id)
        {
            return dc.Table<EntityEntity>().Any(e => e.Id == id);
        }
    }
}