﻿using System;
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
using Eagle.Utility;
using Eagle.Model.Extensions;

namespace Eagle.Modules.Entity
{
    [Route("v1/Entities")]
    public class EntitiesController : ControllerBase
    {
        private readonly DataContexts _context = new DataContexts();

        // GET: api/Entities
        [HttpGet("{agentId}/Query")]
        public PageResultModel<EntityModel> GetEntities(string agentId, [FromQuery] string name, [FromQuery] int page = 1)
        {
            var query = _context.Entities.Where(x => x.AgentId == agentId);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var total = query.Count();

            int size = 20;

            var items = query.Skip((page - 1) * size).Take(size).Select(x => x.Map<EntityModel>()).ToList();
            return new PageResultModel<EntityModel> { Total = total, Page = page, Size = size, Items = items };
        }

        // GET: v1/Entities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntity([FromRoute] string id)
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

            var entity = _context.Entities.Find(id).Map<EntityModel>();

            var items = (from entry in _context.EntityEntries
                         join synonym in _context.EntityEntrySynonyms on entry.Id equals synonym.EntityEntryId
                         where entry.EntityId == id
                         select new { synonym.EntityEntryId, synonym.Synonym, entry.Value, }).ToList();

            entity.Entries = items.Select(x => new EntityEntryModel
            {
                Value = x.Value,
                Id = x.EntityEntryId,
                Synonyms = items.Where(syn => syn.EntityEntryId == x.EntityEntryId)
                .Select(syn => syn.Synonym)
                .ToList()
            });

            return Ok(entity);
        }

        // PUT: api/Entities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntities([FromRoute] string id, [FromBody] EntityModel entityModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entityModel.Id)
            {
                return BadRequest();
            }

            _context.Transaction(delegate {
                entityModel.Update(_context);
            });

            return Ok(entityModel.Id);
        }

        // POST: v1/Entities
        [HttpPost("{agentId}")]
        public async Task<IActionResult> PostEntities(string agentId, [FromBody] EntityModel entityModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Transaction(delegate {
                entityModel.AgentId = agentId;
                entityModel.Add(_context);
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

            var entities = await _context.Entities.SingleOrDefaultAsync(m => m.Id == id);
            if (entities == null)
            {
                return NotFound();
            }

            _context.Transaction(delegate {
                entities.Map<EntityModel>().Delete(_context);
            });

            return Ok(entities.Id);
        }

        private bool EntitiesExists(string id)
        {
            return _context.Entities.Any(e => e.Id == id);
        }
    }
}