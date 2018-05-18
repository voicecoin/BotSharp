using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.Core.Chatbots.Tuling
{
    public class TulingResponseResult
    {
        public int GroupType { get; set; }
        public string ResultType { get; set; }

        public TulingResponseResultValue Values { get; set; }
    }
}
