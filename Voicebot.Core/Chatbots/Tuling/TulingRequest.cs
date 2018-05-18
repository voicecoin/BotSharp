using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.Core.Chatbots.Tuling
{
    public class TulingRequest
    {
        public int ReqType { get; set; }
        public TulingRequestPerception Perception { get; set; }
        public TulingRequestUserInfo UserInfo { get; set; }
    }


}
