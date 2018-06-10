using BotSharp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    public class VmEntityType
    {
        public String Id { get; set; }

        public String AgentId { get; set; }

        public String Color { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public List<VmEntityEntry> Entries { get; set; }

        public Boolean IsEnum { get; set; }

        public EntityType ToEntityType(EntityType model = null)
        {
            if (model == null)
            {
                model = new EntityType
                {
                    Id = Guid.NewGuid().ToString(),
                    AgentId = AgentId
                };
            }

            model.Color = Color;
            model.Name = Name;
            model.Description = Description;
            model.IsEnum = IsEnum;
            model.Entries = Entries.Select(x => new EntityEntry
            {
                Id = Guid.NewGuid().ToString(),
                Value = x.Value,
                Synonyms = x.Synonyms.Select(synonym => new EntrySynonym {
                    Synonym = synonym
                }).ToList()
            }).ToList();

            return model;
        }
    }
}
