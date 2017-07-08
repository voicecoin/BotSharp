using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DomainModels
{
    public class DmAgent
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String UserId { get; set; }
        public String ClientAccessToken { get; set; }
        public DateTime CreatedDate { get; set; }
        public String Language { get; set; }
        public Boolean IsPublic { get; set; }
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
