using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.Core.Voicechain
{
    public class VoicechainResponse<T>
    {
        public int Code { get; set; }
        public String Message { get; set; }
        public T Data { get; set; }
    }
}
