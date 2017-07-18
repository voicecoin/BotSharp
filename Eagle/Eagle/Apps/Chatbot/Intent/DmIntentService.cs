using Eagle.DbTables;
using Eagle.DomainModels;
using Eagle.DataContexts;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Apps.Chatbot.DomainModels;

namespace Eagle.Apps.Chatbot.DmServices
{
    public static partial class DmIntentService
    {
        public static void Load(this DmIntent intentModel, CoreDbContext dc)
        {
            var intentExpressions = dc.Chatbot_IntentExpressions.Where(x => x.IntentId == intentModel.Id).ToList();
            var intentExpressionItems = (from item in dc.Chatbot_IntentExpressions
                                         where intentExpressions.Select(expression => expression.Id).Contains(item.Id)
                                         select item).ToList();

            intentModel.UserSays = intentExpressions.Select(expression => new DmIntentExpression
            {
                Id = expression.Id,
                Text = expression.Text,
                IntentId = expression.IntentId,
                Data = expression.Items.ToList()
            }).ToList();

            intentModel.Templates = intentModel.UserSays.Select(x => x.Data.GetTemplateString()).ToList();

            intentModel.Responses = dc.Chatbot_IntentResponses
                .Where(x => x.IntentId == intentModel.Id)
                .Select(x => x.Map<DmIntentResponse>())
                .ToList();

            intentModel.Responses.ForEach(response =>
            {
                // Load message
                response.Messages = dc.Chatbot_IntentResponseMessages.Where(x => x.IntentResponseId == response.Id)
                    .Select(x => x.Map<DmIntentResponseMessage>()).ToList();

                // Load parameters
                response.Parameters = dc.Chatbot_IntentResponseParameters.Where(x => x.IntentResponseId == response.Id)
                                    .Select(x => x.Map<DmIntentResponseParameter>()).ToList();
            });
        }


        public static void Add(this DmIntent intentModel, CoreDbContext dc)
        {
            if (String.IsNullOrEmpty(intentModel.Id))
            {
                intentModel.Id = Guid.NewGuid().ToString();
            }
            
            var intentRecord = intentModel.Map<Intents>();
            intentRecord.CreatedDate = DateTime.UtcNow;
            intentRecord.Contexts = intentModel.Contexts.ToArray();

            dc.Chatbot_Intents.Add(intentRecord);

            intentModel.UserSays.ForEach(userSay =>
            {
                userSay.IntentId = intentRecord.Id;
                userSay.Add(dc);
            });

            intentModel.Responses.ForEach(response =>
            {
                response.IntentId = intentRecord.Id;
                response.Add(dc);
            });
        }

        public static void Update(this DmIntent intentModel, CoreDbContext dc)
        {
            var intentRecord = dc.Chatbot_Intents.Find(intentModel.Id);
            intentRecord.Name = intentModel.Name;
            intentRecord.ModifiedDate = DateTime.UtcNow;

            // Remove all related data then create with same IntentId
            intentModel.UserSays.ForEach(expression => expression.Update(dc));
            intentModel.Responses.ForEach(response => response.Update(dc));
        }
    }
}
