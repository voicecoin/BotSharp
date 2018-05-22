using BotSharp.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    public class VmIntentResponseMessage
    {
        public AIResponseMessageType Type { get; set; }

        public String Lang { get; set; }

        public String Speech { get; set; }

        public JObject Payload { get; set; }
    }
}
