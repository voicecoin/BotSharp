using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Intent view model
    /// </summary>
    public class VmIntent
    {
        public String Id { get; set; }

        public String AgentId { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }
    }
}
