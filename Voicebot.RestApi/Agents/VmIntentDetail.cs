using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Intent detail view model, used in Create/ Update
    /// </summary>
    public class VmIntentDetail
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Auto { get; set; }

        public List<String> Events { get; set; }

        public List<VmIntentResponse> Responses { get; set; }

        public List<VmIntentExpression> UserSays { get; set; }
    }
}
