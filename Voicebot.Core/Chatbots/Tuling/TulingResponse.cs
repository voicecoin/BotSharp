using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.Core.Chatbots.Tuling
{
    public class TulingResponse
    {
        public TulingIntent Intent { get; set; }
        public List<TulingResponseResult> Results { get; set; }
    }
}
