using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Agent view model
    /// </summary>
    public class VmAgent
    {
        public String Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Boolean Published { get; set; }

        public String Language { get; set; }

        public String Birthday { get; set; }
    }
}
