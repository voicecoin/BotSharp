using BotSharp.Core.Intents;
using DotNetToolkit;
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

        public Boolean ResetContexts { get; set; }

        public List<VmIntentResponseContext> AffectedContexts { get; set; }

        public List<VmIntentResponseParameter> Parameters { get; set; }

        public List<VmIntentResponseMessage> Messages { get; set; }

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
