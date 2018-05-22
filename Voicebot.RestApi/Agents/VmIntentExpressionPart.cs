using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    public class VmIntentExpressionPart
    {
        public String Text { get; set; }

        /// <summary>
        /// Entity type name
        /// </summary>
        public String Alias { get; set; }

        public String Meta { get; set; }

        public Boolean UserDefined { get; set; }
    }
}
