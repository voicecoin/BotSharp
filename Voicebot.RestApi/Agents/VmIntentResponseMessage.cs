using BotSharp.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    public class VmIntentResponseMessage
    {
        public VmIntentResponseMessage()
        {
            Speeches = new List<string>();
        }

        public AIResponseMessageType Type { get; set; }

        public List<String> Speeches { get; set; }

        public JObject Payload { get; set; }
    }
}
