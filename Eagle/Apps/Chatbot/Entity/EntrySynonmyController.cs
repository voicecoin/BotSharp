﻿using Core;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Chatbot_ConversationParameters.Entity
{
    public class EntrySynonmyController : CoreController
    {
        [HttpPost("{entryId}")]
        public IActionResult PostEntrySynonmy(string entryId, [FromBody] EntityEntrySynonymEntity synonymModel)
        {
            var dm = new DomainModel<EntityEntrySynonymEntity>(dc, synonymModel);
            dm.ValideModel(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dc.Transaction<IDbRecord4SqlServer>(delegate {
                dm.AddEntity();
            });

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

            dc.Transaction<IDbRecord4SqlServer>(delegate {
                dc.Table<EntityEntrySynonymEntity>().Remove(entitySynonym);
            });

            return Ok(entitySynonym.Id);
        }
    }
}
