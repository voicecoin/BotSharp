using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Chatbot.DomainModels
{
    public class DmEntity
    {
        public String Id { get; set; }
        public String AgentId { get; set; }
        public String Name { get; set; }
        public Boolean IsEnum { get; set; }
        public Boolean IsOverridable { get; set; }
        public String Color { get; set; }
        public IEnumerable<DmEntityEntry> Entries { get; set; }
    }

    public class DmEntityEntry
    {
        public String Id { get; set; }
        public String EntityId { get; set; }
        public String Value { get; set; }
        public String Template { get; set; }
        public IEnumerable<String> Synonyms { get; set; }
    }
}
