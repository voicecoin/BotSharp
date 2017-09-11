using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core;
using Apps.Chatbot.DomainModels;
using Utility;
using Apps.Chatbot.DmServices;
using Core.Interfaces;
using Enyim.Caching;

namespace Apps.Chatbot.Entity
{
    public class EntityEntriesController : CoreController
    {
        // GET: v1/EntityEntries
        [HttpGet("{entityId}/Query")]
        public DmPageResult<EntityEntryEntity> GetEntityEntries(string entityId, string name, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var query = dc.Table<EntityEntryEntity>().Where(x => x.EntityId == entityId);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Value.Contains(name));
            }

            var total = query.Count();

            var items = query.Skip((page - 1) * size).Take(size).ToList();

            var synonyms = (from synonym in dc.Table<EntityEntrySynonymEntity>()
                         where items.Select(x => x.Id).Contains(synonym.EntityEntryId)
                         select new { synonym.EntityEntryId, synonym.Synonym}).ToList();

            items.ForEach(item => {
                item.Synonyms = synonyms.Where(x => x.EntityEntryId == item.Id).Select(x => x.Synonym);
            });

            return new DmPageResult<EntityEntryEntity> { Total = total, Page = page, Size = size, Items = items };
        }

        // GET: v1/EntityEntries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntityEntries([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityEntries = await dc.Table<EntityEntryEntity>().SingleOrDefaultAsync(m => m.Id == id);

            if (entityEntries == null)
            {
                return NotFound();
            }

            return Ok(entityEntries);
        }

        // PUT: api/EntityEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntityEntries([FromRoute] string id, [FromBody] EntityEntryEntity entityEntryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entityEntryModel.Id)
            {
                return BadRequest();
            }

            dc.Transaction<IDbRecord4Core>(delegate {
                entityEntryModel.Update(dc);
            });

            return Ok(entityEntryModel.Id);
        }

        // POST: api/EntityEntries
        [HttpPost("{entityId}")]
        public async Task<IActionResult> PostEntityEntry(string entityId, [FromBody] EntityEntryEntity entityEntryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dc.Transaction<IDbRecord4Core>(delegate {
                entityEntryModel.EntityId = entityId;
                entityEntryModel.Add(dc);
            });

            return CreatedAtAction("GetEntityEntries", new { id = entityEntryModel.Id }, entityEntryModel);
        }

        // DELETE: api/EntityEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntityEntries([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityEntries = await dc.Table<EntityEntryEntity>().SingleOrDefaultAsync(m => m.Id == id);
            if (entityEntries == null)
            {
                return NotFound();
            }

            dc.Transaction<IDbRecord4Core>(delegate {
                entityEntries.Map<EntityEntryEntity>().Delete(dc);
            });

            return Ok(entityEntries.Id);
        }

        private bool EntityEntriesExists(string id)
        {
            return dc.Table<EntityEntryEntity>().Any(e => e.Id == id);
        }
    }
}