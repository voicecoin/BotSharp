using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Intent;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Apps.Chatbot.DmServices
{
    public static partial class DmIntentService
    {
        public static void Load(this DmIntent intentModel, CoreDbContext dc)
        {
            var intentExpressions = dc.Table<IntentExpressionEntity>().Where(x => x.IntentId == intentModel.Id).ToList();
            var intentExpressionItems = (from item in dc.Table<IntentExpressionEntity>()
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

            intentModel.Responses = dc.Table<IntentResponseEntity>()
                .Where(x => x.IntentId == intentModel.Id)
                .Select(x => x.Map<DmIntentResponse>())
                .ToList();

            intentModel.Responses.ForEach(response =>
            {
                // Load message
                response.Messages = dc.Table<IntentResponseMessageEntity>().Where(x => x.IntentResponseId == response.Id)
                    .Select(x => x.Map<DmIntentResponseMessage>()).ToList();

                // Load parameters
                response.Parameters = dc.Table<IntentResponseMessageEntity>().Where(x => x.IntentResponseId == response.Id)
                                    .Select(x => x.Map<DmIntentResponseParameter>()).ToList();
            });
        }


        public static void Add(this DmIntent intentModel, CoreDbContext dc)
        {
            if (String.IsNullOrEmpty(intentModel.Id))
            {
                intentModel.Id = Guid.NewGuid().ToString();
            }
            
            var intentRecord = intentModel.Map<IntentEntity>();
            intentRecord.CreatedDate = DateTime.UtcNow;
            intentRecord.CreatedUserId = dc.CurrentUser.Id;
            intentRecord.ModifiedDate = DateTime.UtcNow;
            intentRecord.ModifiedUserId = dc.CurrentUser.Id;
            intentRecord.Contexts = intentModel.Contexts.ToArray();

            dc.Table<IntentEntity>().Add(intentRecord);

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
            var intentRecord = dc.Table<IntentEntity>().Find(intentModel.Id);
            intentRecord.Name = intentModel.Name;
            intentRecord.ModifiedDate = DateTime.UtcNow;

            // Remove all related data then create with same IntentId
            intentModel.UserSays.ForEach(expression => expression.Update(dc));
            intentModel.Responses.ForEach(response => response.Update(dc));
        }
    }
}
