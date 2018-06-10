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
    [Route("/v1/Entities")]
    public class EntityTypeController : CoreController
    {
        [HttpGet("{agentId}/query")]
        public PageResult<EntityType> Query([FromRoute] string agentId, [FromQuery] int page = 1, [FromQuery] string name = "")
        {
            var result = new PageResult<EntityType>() { Page = page };
            var query = dc.Table<EntityType>().Include(x => x.Entries)
                .Where(x => x.AgentId == agentId);

            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }

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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("{agentId}")]
        public IActionResult CreateEntityType([FromRoute] string agentId, [FromBody] VmEntityType model)
        {
            if (model == null) return BadRequest("entity is empty");

            model.AgentId = agentId;
            model.Id = Guid.NewGuid().ToString();

            dc.DbTran(() => {

                var entity = new EntityType
                {
                    Name = model.Name,
                    AgentId = model.AgentId,
                    IsEnum = model.IsEnum,
                    Description = model.Description,
                    Entries = model.Entries.Select(x => new EntityEntry
                    {
                        Value = x.Value,
                        Synonyms = x.Synonyms.Select(synonym => new EntrySynonym
                        {
                            Synonym = synonym
                        }).ToList()
                    }).ToList()
                };

                dc.Table<EntityType>().Add(entity);
            });

            return Ok(model.Id);
        }

        [HttpPut("{entityTypeId}")]
        public String UpdateEntityType([FromRoute] string entityTypeId, [FromBody] VmEntityType model)
        {
            dc.DbTran(() => {

                // remove
                var entityType = dc.Table<EntityType>().Find(entityTypeId);
                dc.Table<EntityType>().Remove(entityType);
                dc.SaveChanges();

                // add back
                entityType = model.ToEntityType(entityType);
                entityType.UpdatedTime = DateTime.UtcNow;

                dc.Table<EntityType>().Add(entityType);
            });

            return model.Id;
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
