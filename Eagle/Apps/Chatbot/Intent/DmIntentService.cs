using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Intent;
using Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Apps.Chatbot.DmServices
{
    public static partial class DmIntentService
    {
        public static void Load(this DomainModel<IntentEntity> intentModel)
        {
            CoreDbContext dc = intentModel.Dc;
            intentModel.LoadEntity();

            var intentExpressions = dc.Table<IntentExpressionEntity>().Where(x => x.IntentId == intentModel.Entity.Id).ToList();

            intentModel.Entity.UserSays = intentExpressions.Select(expression => new IntentExpressionEntity
            {
                Id = expression.Id,
                Text = expression.Text,
                IntentId = expression.IntentId,
                Data = expression.Data.ToList()
            }).ToList();

            intentModel.Entity.Templates = intentModel.Entity.UserSays.Select(x => x.Data.GetTemplateString()).ToList();

            intentModel.Entity.Responses = dc.Table<IntentResponseEntity>()
                .Where(x => x.IntentId == intentModel.Entity.Id)
                .ToList();

            intentModel.Entity.Responses.ForEach(response =>
            {
                // Load message
                response.Messages = dc.Table<IntentResponseMessageEntity>().Where(x => x.IntentResponseId == response.Id)
                    .Select(x => x.Map<IntentResponseMessageEntity>()).ToList();

                // Load parameters
                response.Parameters = dc.Table<IntentResponseParameterEntity>().Where(x => x.IntentResponseId == response.Id)
                                    .Select(x => x.Map<IntentResponseParameterEntity>()).ToList();
            });
        }


        public static void Add(this DomainModel<IntentEntity> intentModel)
        {
            if (!intentModel.AddEntity()) return;
            intentModel.Entity.ContextsJson = JsonConvert.SerializeObject(intentModel.Entity.Contexts);

            intentModel.Entity.UserSays.ForEach(userSay =>
            {
                userSay.IntentId = intentModel.Entity.Id;
                var dm = new DomainModel<IntentExpressionEntity>(intentModel.Dc, userSay);
                dm.Add();
            });

            intentModel.Entity.Responses.ForEach(response =>
            {
                response.IntentId = intentModel.Entity.Id;
                var dm = new DomainModel<IntentResponseEntity>(intentModel.Dc, response);
                dm.Add();
            });
        }

        public static void Update(this DomainModel<IntentEntity> intentModel)
        {
            CoreDbContext dc = intentModel.Dc;
            var intentRecord = dc.Table<IntentEntity>().Find(intentModel.Entity.Id);
            intentRecord.Name = intentModel.Entity.Name;
            intentRecord.ModifiedDate = DateTime.UtcNow;

            // Remove all related data then create with same IntentId
            intentModel.Entity.UserSays.ForEach(expression => new DomainModel<IntentExpressionEntity>(dc, expression).Update());
            intentModel.Entity.Responses.ForEach(response => new DomainModel<IntentResponseEntity>(dc, response).Update());
        }
    }
}
