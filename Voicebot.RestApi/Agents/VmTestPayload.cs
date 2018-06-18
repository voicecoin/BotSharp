using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Voicebot.Core.Voicechain;

namespace Voicebot.RestApi.Agents
{
    public class VmTestPayload
    {
        public string ConversationId { get; set; }
        public string FulfillmentText { get; set; }
        public string Sender { get; set;}
        public Object Payload { get; set; }
    }
}
