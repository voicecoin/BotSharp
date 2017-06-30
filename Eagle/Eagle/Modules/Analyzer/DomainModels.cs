using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Models
{
    public class DeepParsedModel
    {
        public List<IntentExpressionItemModel> Tags { get; set; }
    }

    public class AnalyzerModel
    {
        public string Text { get; set; }
        public string ClientAccessToken { get; set; }
        public string SessionId { get; set; }
    }
}
