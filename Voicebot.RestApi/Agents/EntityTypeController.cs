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
    public class EntityTypeController : CoreController
    {
        [HttpGet("{agentId}/query")]
        public PageResult<EntityType> Query([FromRoute] string agentId)
        {
            var result = new PageResult<EntityType>() { };
            var query = dc.Table<EntityType>().Include(x => x.Entries)
                .Where(x => x.AgentId == agentId);

            var items = result.LoadRecords<EntityType>(query);
            items.Items.ForEach(x => x.Count = x.Entries.Count());

            return items;
        }

        [HttpGet("{entityTypeId}")]
        public EntityType GetEntityType([FromRoute] string entityTypeId)
        {
            return dc.Table<EntityType>().Find(entityTypeId);
        }

        /// <summary>
        /// Define a new entity type
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("{agentId}")]
        public EntityType CreateEntityType([FromRoute] string agentId, [FromBody] EntityType entity)
        {
            dc.DbTran(() => {
                entity.AgentId = agentId;
                entity.Id = Guid.NewGuid().ToString();
                dc.Table<EntityType>().Add(entity);
            });

            return entity;
        }

        [HttpPut("{entityTypeId}")]
        public String UpdateEntityType([FromRoute] string entityTypeId, [FromBody] EntityType entity)
        {
            dc.DbTran(() => {
                var existed = dc.Table<EntityType>().Find(entityTypeId);
                existed.Color = entity.Color;
                existed.Name = entity.Name;
                existed.Description = entity.Description;
                existed.IsEnum = entity.IsEnum;
                existed.UpdatedTime = DateTime.UtcNow;
            });

            return entity.Id;
        }

        [HttpDelete("{entityTypeId}")]
        public String DeleteEntityType([FromRoute] string entityTypeId)
        {
            dc.DbTran(() => {
                var entity = dc.Table<EntityType>().Find(entityTypeId);
                dc.Table<EntityType>().Remove(entity);
            });

            return entityTypeId;
        }
    }
}
