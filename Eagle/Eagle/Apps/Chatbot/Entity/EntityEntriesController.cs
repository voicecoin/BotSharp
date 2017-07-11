﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eagle.DomainModels;
using Eagle.Utility;
using Eagle.DmServices;
using Eagle.Core;
using Eagle.Apps.Chatbot.DomainModels;
using Eagle.Apps.Chatbot.DmServices;

namespace Eagle.Apps.Chatbot.Entity
{
    public class EntityEntriesController : CoreController
    {
        // GET: v1/EntityEntries
        [HttpGet("{entityId}/Query")]
        public DmPageResult<DmEntityEntry> GetEntityEntries(string entityId, string name, [FromQuery] int page = 1)
        {
            var query = dc.EntityEntries.Where(x => x.EntityId == entityId);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Value.Contains(name));
            }

            var total = query.Count();

            int size = 20;

            var items = query.Skip((page - 1) * size).Take(size).Select(x => x.Map<DmEntityEntry>()).ToList();

            var synonyms = (from synonym in dc.EntityEntrySynonyms
                         where items.Select(x => x.Id).Contains(synonym.EntityEntryId)
                         select new { synonym.EntityEntryId, synonym.Synonym}).ToList();

            items.ForEach(item => {
                item.Synonyms = synonyms.Where(x => x.EntityEntryId == item.Id).Select(x => x.Synonym);
            });

            return new DmPageResult<DmEntityEntry> { Total = total, Page = page, Size = size, Items = items };
        }

        // GET: v1/EntityEntries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntityEntries([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityEntries = await dc.EntityEntries.SingleOrDefaultAsync(m => m.Id == id);

            if (entityEntries == null)
            {
                return NotFound();
            }

            return Ok(entityEntries);
        }

        // PUT: api/EntityEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntityEntries([FromRoute] string id, [FromBody] DmEntityEntry entityEntryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entityEntryModel.Id)
            {
                return BadRequest();
            }

            dc.Transaction(delegate {
                entityEntryModel.Update(dc);
            });

            return Ok(entityEntryModel.Id);
        }

        // POST: api/EntityEntries
        [HttpPost("{entityId}")]
        public async Task<IActionResult> PostEntityEntry(string entityId, [FromBody] DmEntityEntry entityEntryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dc.Transaction(delegate {
                entityEntryModel.EntityId = entityId;
                entityEntryModel.Add(dc);
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

            var entityEntries = await dc.EntityEntries.SingleOrDefaultAsync(m => m.Id == id);
            if (entityEntries == null)
            {
                return NotFound();
            }

            dc.Transaction(delegate {
                entityEntries.Map<DmEntityEntry>().Delete(dc);
            });

            return Ok(entityEntries.Id);
        }

        private bool EntityEntriesExists(string id)
        {
            return dc.EntityEntries.Any(e => e.Id == id);
        }
    }
}