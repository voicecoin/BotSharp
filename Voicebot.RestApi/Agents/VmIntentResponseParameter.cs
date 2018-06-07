using BotSharp.Core.Intents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    public class VmIntentResponseParameter
    {
        public String Id { get; set; }

        public bool Required { get; set; }

        public string DataType { get; set; }

        public string DefaultValue { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public List<String> Prompts { get; set; }

        public bool IsList { get; set; }

        public static VmIntentResponseParameter FromIntentResponseParameter(IntentResponseParameter parameter)
        {
            var p = new VmIntentResponseParameter
            {
                DataType = parameter.DataType,
                DefaultValue = parameter.DefaultValue,
                IsList = parameter.IsList,
                Name = parameter.Name,
                Required = parameter.Required,
                Value = parameter.Value,
                Prompts = parameter.Prompts.Select(x => x.Prompt).ToList()
            };

            return p;
        }
    }
}
