using BotSharp.Core.Intents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    public class VmIntentExpression
    {
        public string Id { get; set; }

        public bool IsTemplate { get; set; }

        public bool IsAuto { get; set; }

        public List<VmIntentExpressionPart> Data { get; set; }

        public string Speech
        {
            get
            {
                return String.Join(String.Empty, Data.Select(x => x.Text));
            }
        }

        public IntentExpression ToIntentExpression(IntentExpression expression = null)
        {
            if(expression == null)
            {
                expression = new IntentExpression
                {
                    Id = Guid.NewGuid().ToString(),
                    Data = Data.Select(x => x.ToIntentExpressionPart()).ToList()
                };
            }

            return expression;
        }
    }
}
