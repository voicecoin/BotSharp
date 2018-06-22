using System;
using System.Collections.Generic;
using System.Text;
using Voicebot.Core.Voicechain;

namespace Voicebot.RestApi.Agents
{
    public class VmAgentDetail : VmAgent
    {
        /// <summary>
        /// Ingore if creating
        /// </summary>
        public ANameModel VNS { get; set; }

        public String ClientAccessToken { get; set; }

        public String DeveloperAccessToken { get; set; }
    }
}
