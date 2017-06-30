using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.Models;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DddServices
{
    public static class DddAgentService
    {
        public static AgentResponseModel TextRequest(this AgentModel agentModel, DataContexts dc)
        {
            var model = new AnalyzerModel { Text = agentModel.Request.Text };
            List<IntentExpressionItemModel> items = model.Ner(dc);

            return new AgentResponseModel { Text = "TEST" };
        }
    }
}
