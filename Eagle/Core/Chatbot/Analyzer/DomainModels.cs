using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Chatbot.DomainModels
{
    public class DmDeepParsed
    {
        public List<DmIntentExpressionItem> Tags { get; set; }
    }

    public class NlpirResult
    {
        public List<NlpirSegment> WordSplit { get; set; }
    }

    public class NlpirSegment
    {
        public string Word { get; set; }
        public string Entity { get; set; }
    }
}
