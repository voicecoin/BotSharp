using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Models
{
    public class AgentModel
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String UserId { get; set; }
        public String ClientAccessToken { get; set; }
    }

    public class AgentDetailModel : AgentModel
    {
        public String Language { get; set; }
        public Boolean IsPublic { get; set; }
    }
}
