using BotSharp.Core.Intents;
using DotNetToolkit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Intent response
    /// </summary>
    public class VmIntentResponse
    {
        public VmIntentResponse()
        {
            AffectedContexts = new List<VmIntentResponseContext>();
            Parameters = new List<VmIntentResponseParameter>();
            Messages = new List<VmIntentResponseMessage>();
        }

        public String Action { get; set; }

        public Boolean ResetContexts { get; set; }

        public List<VmIntentResponseContext> AffectedContexts { get; set; }

        public List<VmIntentResponseParameter> Parameters { get; set; }

        public List<VmIntentResponseMessage> Messages { get; set; }

        public static VmIntentResponse FromIntentResponse(IntentResponse intentResponse)
        {
            var response = new VmIntentResponse
            {
                Action = intentResponse.Action
            };

            response.AffectedContexts = intentResponse.Contexts.Select(ctx => ctx.ToObject<VmIntentResponseContext>()).ToList();

            response.Parameters = intentResponse.Parameters.Select(p => VmIntentResponseParameter.FromIntentResponseParameter(p)).ToList();

            response.Messages = intentResponse.Messages.Select(msg => {

                if (msg.Speech == null) return new VmIntentResponseMessage();

                return new VmIntentResponseMessage
                {
                    Payload = msg.Payload,
                    Type = msg.Type,
                    Speeches = JsonConvert.DeserializeObject<List<String>>(msg.Speech)
                };

            }).ToList();

            return response;
        }

        public IntentResponse ToIntentResponse(IntentResponse intentResponse = null)
        {
            if(intentResponse == null)
            {
                intentResponse = new IntentResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    ResetContexts = ResetContexts,
                    Contexts = AffectedContexts.Select(x => x.ToObject<IntentResponseContext>()).ToList(),
                    Parameters = Parameters.Select(x => x.ToObject<IntentResponseParameter>()).ToList(),
                    Messages = Messages.Select(x => x.ToIntentResponseMessage()).ToList()
                };
            }

            return intentResponse;
        }
    }
}
