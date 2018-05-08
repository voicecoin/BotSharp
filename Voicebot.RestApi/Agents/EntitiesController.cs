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
    public class EntitiesController : CoreController
    {
        [HttpGet("{agentId}/query")]
        public PageResult<Entity> Query([FromRoute] string agentId)
        {
            var result = new PageResult<Entity>() { };
            var query = dc.Table<Entity>().Include(x => x.Entries)
                .Where(x => x.AgentId == agentId);

            var items = result.LoadRecords<Entity>(query);
            items.Items.ForEach(x => x.Count = x.Entries.Count());

            return items;
        }

        [HttpGet("{entityId}")]
        public Entity GetEntity([FromRoute] string entityId)
        {
            return dc.Table<Entity>().Find(entityId);
        }

        [HttpPost("{agentId}")]
        public Entity CreateEntity([FromRoute] string agentId, [FromBody] Entity entity)
        {
            dc.DbTran(() => {
                entity.AgentId = agentId;
                entity.Id = Guid.NewGuid().ToString();
                dc.Table<Entity>().Add(entity);
            });

            return entity;
        }

        [HttpPut("{entityId}")]
        public String UpdateEntity([FromRoute] string entityId, [FromBody] Entity entity)
        {
            dc.DbTran(() => {
                var existed = dc.Table<Entity>().Find(entityId);
                existed.Color = entity.Color;
                existed.Name = entity.Name;
                existed.Description = entity.Description;
                existed.IsEnum = entity.IsEnum;
                existed.UpdatedTime = DateTime.UtcNow;
            });

            return entity.Id;
        }

        [HttpDelete("{entityId}")]
        public String DeleteEntity([FromRoute] string entityId)
        {
            dc.DbTran(() => {
                var entity = dc.Table<Entity>().Find(entityId);
                dc.Table<Entity>().Remove(entity);
            });

            return entityId;
        }
    }
}
