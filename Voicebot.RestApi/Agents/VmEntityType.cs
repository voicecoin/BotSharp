using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    public class VmEntityType
    {
        public String Id { get; set; }

        public String AgentId { get; set; }

        public String Name { get; set; }

        public List<VmEntityEntry> Entries { get; set; }

        public Boolean IsEnum { get; set; }
    }
}
