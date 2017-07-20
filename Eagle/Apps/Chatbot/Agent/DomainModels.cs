using Apps.Chatbot.Agent;
using Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Chatbot.DomainModels
{
    public class DmAgent : BundleDbRecord
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public String Language { get; set; }

        public String UserId { get; set; }
        /// <summary>
        /// 给Yaya平台调用的。
        /// </summary>
        public String ClientAccessToken { get; set; }
        /// <summary>
        /// 给SDK调用的
        /// </summary>
        public String DeveloperAccessToken { get; set; }

        public bool IsPublic { get; set; }
        public String Avatar { get; set; }
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
