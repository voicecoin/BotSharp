using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eagle.DomainModels;
using AutoMapper;
using Eagle.Utility;
using Eagle.DmServices;
using Eagle.Core;
using Eagle.Apps.Chatbot.DomainModels;
using Eagle.Apps.Chatbot.DmServices;

namespace Eagle.Apps.Chatbot.Entity
{
    public class EntitiesController : CoreController
    {
        // GET: api/Entities
        [HttpGet("{agentId}/Query")]
        public DmPageResult<DmEntity> GetEntities(string agentId, [FromQuery] string name, [FromQuery] int page = 1)
        {
            var query = dc.Entities.Where(x => x.AgentId == agentId);
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

            var entities = await dc.Entities.SingleOrDefaultAsync(m => m.Id == id);

            if (entities == null)
            {
                return NotFound();
            }

            var entity = dc.Entities.Find(id).Map<DmEntity>();

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

            dc.Transaction(delegate {
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

            dc.Transaction(delegate {
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

            var entities = await dc.Entities.SingleOrDefaultAsync(m => m.Id == id);
            if (entities == null)
            {
                return NotFound();
            }

            dc.Transaction(delegate {
                entities.Map<DmEntity>().Delete(dc);
            });

            return Ok(entities.Id);
        }

        private bool EntitiesExists(string id)
        {
            return dc.Entities.Any(e => e.Id == id);
        }
    }
}