using BotSharp.Core.Entities;
using DotNetToolkit;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    public class EntityEntriesController : CoreController
    {
        [HttpGet("{entityId}/Query")]
        public PageResult<EntityEntry> Query([FromRoute] string entityId)
        {
            var result = new PageResult<EntityEntry>() { };
            var query = dc.Table<EntityEntry>()
                .Include(x => x.Synonyms)
                .Where(x => x.EntityId == entityId);

            var items = result.LoadRecords<EntityEntry>(query);

            items.Items.ForEach(x =>
            {
                if (x.Synonyms == null)
                {
                    x.Synonyms = new List<EntrySynonym>();
                }
            });

            return items;
        }

        [HttpPost("{entityId}")]
        public EntityEntry CreateEntityEntry([FromRoute] string entityId, EntityEntry entityEntry)
        {
            dc.DbTran(() =>
            {
                entityEntry.EntityId = entityId;
                dc.Table<EntityEntry>().Add(entityEntry);
            });

            return entityEntry;
        }

        [HttpPut("{entityEntryId}")]
        public String UpdateEntityEntry([FromRoute] string entityEntryId, [FromBody] EntityEntry entityEntry)
        {
            dc.DbTran(() => {
                var entry = dc.Table<EntityEntry>().Find(entityEntryId);
                entry.Value = entityEntry.Value;
            });

            return entityEntry.Id;
        }

        [HttpDelete("{entityEntryId}")]
        public String DeleteEntityEntry([FromRoute] string entityEntryId)
        {
            dc.DbTran(() => {
                var entry = dc.Table<EntityEntry>().Find(entityEntryId);
                dc.Table<EntityEntry>().Remove(entry);
            });

            return entityEntryId;
        }
    }
}
