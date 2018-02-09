using Core;
using Core.Interfaces;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Chatbot.Entity
{
    public class EntrySynonmyController : CoreController
    {
        [HttpPost("{entryId}")]
        public IActionResult PostEntrySynonmy(string entryId, [FromBody] EntityEntrySynonymEntity synonymModel)
        {
            /*EntityEntrySynonymEntity dm = new DomainModel<EntityEntrySynonymEntity>(dc, synonymModel);
            dm.ValideModel(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dc.Transaction<IDbRecord>(delegate {
                dm.AddEntity();
            });*/

            return CreatedAtAction("GetEntityEntries", new { id = synonymModel.Id }, synonymModel.Synonym);
        }

        [HttpDelete("{entryId}/{synonym}")]
        public IActionResult DeleteEntrySynonmy([FromRoute] string entryId, [FromRoute] string synonym)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entitySynonym = dc.Table<EntityEntrySynonymEntity>().FirstOrDefault(m => m.EntityEntryId == entryId && m.Synonym == synonym);
            if (entitySynonym == null)
            {
                return NotFound();
            }

            dc.Transaction<IDbRecord>(delegate {
                dc.Table<EntityEntrySynonymEntity>().Remove(entitySynonym);
            });

            return Ok(entitySynonym.Id);
        }
    }
}
