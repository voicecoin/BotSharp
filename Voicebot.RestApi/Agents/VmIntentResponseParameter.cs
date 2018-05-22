using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    public class VmIntentResponseParameter
    {
        public String Id { get; set; }

        public bool Required { get; set; }

        public string DataType { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public List<String> Prompts { get; set; }

        public bool IsList { get; set; }
    }
}
