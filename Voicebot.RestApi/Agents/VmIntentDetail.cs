using BotSharp.Core.Intents;
using DotNetToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Intent detail view model, used in Create/ Update
    /// </summary>
    public class VmIntentDetail
    {
        public string Id { get; set; }

        public string AgentId { get; set; }

        public string Name { get; set; }

        public bool Auto { get; set; }

        public List<String> Contexts { get; set; }

        public List<String> Events { get; set; }

        public List<VmIntentResponse> Responses { get; set; }

        public List<VmIntentExpression> UserSays { get; set; }

        public Intent ToIntent(Intent intent = null)
        {
            if(intent == null)
            {
                intent = new Intent
                {
                    Id = Guid.NewGuid().ToString(),
                    AgentId = AgentId
                };
            }

            intent.Name = Name;

            intent.Contexts = Contexts.Select(x => new IntentInputContext { Name = x }).ToList();

            intent.UserSays = UserSays.Select(x => x.ToIntentExpression()).ToList();

            intent.Responses = Responses.Select(x => x.ToIntentResponse()).ToList();

            return intent;
        }
    }
}
