using Eagle.DbTables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Apps.Chatbot.DomainModels
{
    public class DmAgent : Agents
    {
    }

    public class DmAgentRequest
    {
        public String Text { get; set; }
        public String ClientAccessToken { get; set; }
        public String SessionId { get; set; }
        public DmAgent Agent { get; set; }
    }

    public class DmAgentResponse
    {
        public String Text { get; set; }
    }
}
