using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.Core.NlpService
{
    public interface INlpEngine
    {
        List<NlpEntity> Ner(string text);
    }
}
