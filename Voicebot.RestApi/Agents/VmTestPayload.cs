using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Voicebot.Core.Voicechain;

namespace Voicebot.RestApi.Agents
{
    public class VmTestPayload
    {
        public string FulfillmentText { get; set; }
        public Object Payload { get; set; }
        public VoicechainResponse<ANameModel> Voicechain { get; set; }
    }
}
