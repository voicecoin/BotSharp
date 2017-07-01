using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.Enums;
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
        public static AgentResponseModel TextRequest(this AgentRequestModel agentRequestModel, DataContexts dc)
        {
            var queryable = from intent in dc.Intents
                            join exp in dc.IntentExpressions on intent.Id equals exp.IntentId
                            where intent.AgentId == agentRequestModel.Agent.Id || intent.AgentId == Constants.GenesisAgentId
                            select exp;

            // 精确匹配
            var intents = queryable.Where(x => x.Text == agentRequestModel.Text).ToList();

            List<IntentExpressionItemModel> items = null;

            if (intents.Count() == 0)
            {
                items = agentRequestModel.PosTagger(dc);
                String template = items.GetTemplateString();
                intents = queryable.Where(x => x.Template == agentRequestModel.Text).ToList();
            }

            if(intents.Count() == 0)
            {
                items.ForEach(item => {
                    /*if (!String.IsNullOrEmpty(item.EntityId))
                    {
                        queryable = from exp in queryable
                                    join item1 in dc.IntentExpressionItems on exp.Id equals item1.IntentExpressionId
                                    where item1.EntityId == item.EntityId
                                    select exp;

                    }*/
                });

                var items1 = queryable.ToList();
            }


            if (intents.Count == 0) return null;

            var intentRecord = dc.Intents.First(m => m.Id == intents.First().IntentId);

            var intentModel = intentRecord.Map<IntentModel>();
            intentModel.Load(dc);

            IntentResponseModel responseModel = intentModel.Responses.First();
            IntentResponseMessageModel messageModel = responseModel.PostResponse(dc, agentRequestModel.Agent);

            return new AgentResponseModel { Text = messageModel.Speech };
        }
    }
}
