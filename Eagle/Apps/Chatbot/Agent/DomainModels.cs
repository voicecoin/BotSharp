using Apps.Chatbot.Agent;
using Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Chatbot.DomainModels
{
    public class DmAgentRequest
    {
        public String Text { get; set; }
        public String ClientAccessToken { get; set; }
        public String ConversationId { get; set; }
        public AgentEntity Agent { get; set; }
    }

    public class DmAgentResponse
    {
        public String Text { get; set; }
    }
}
