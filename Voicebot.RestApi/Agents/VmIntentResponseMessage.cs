using BotSharp.Core.Intents;
using BotSharp.Core.Models;
using Newtonsoft.Json;
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

        public IntentResponseMessage ToIntentResponseMessage(IntentResponseMessage intentResponseMessage = null)
        {
            if (intentResponseMessage == null)
            {
                intentResponseMessage = new IntentResponseMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    Payload = Payload,
                    Type = Type,
                    Speech = JsonConvert.SerializeObject(Speeches)
                };
            }

            return intentResponseMessage;
        }
    }
}
