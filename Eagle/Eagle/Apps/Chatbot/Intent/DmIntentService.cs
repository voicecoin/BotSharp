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
            var intentExpressions = dc.IntentExpressions.Where(x => x.IntentId == intentModel.Id).ToList();
            var intentExpressionItems = (from item in dc.IntentExpressionItems
                                         from entity in dc.Entities.Where(x => item.EntityId == x.Id).DefaultIfEmpty()
                                         where intentExpressions.Select(expression => expression.Id).Contains(item.IntentExpressionId)
                                         orderby item.Position
                                         select new { item, entity }).ToList();

            intentModel.UserSays = intentExpressions.Select(expression => new DmIntentExpression
            {
                Id = expression.Id,
                Text = expression.Text,
                /*Template = intentExpressionItems.Where(item => item.item.IntentExpressionId == expression.Id)
                    .Select(x => {
                        var expressionItem = x.item.Map<DmIntentExpressionItem>();
                        expressionItem.Meta = x.entity.Name;
                        return expressionItem;
                    }).GetTemplateString(),*/
                IntentId = expression.IntentId,
                Data = intentExpressionItems.Where(item => item.item.IntentExpressionId == expression.Id)
                    .Select(item => new DmIntentExpressionItem
                    {
                        EntityId = item.entity == null ? null : item.entity.Id,
                        Meta = item.entity == null ? null : $"@{item.entity?.Name}",
                        Alias = item.item?.Alias,
                        Color = item.entity == null ? String.Empty : item.item.Color,
                        Position = item.item.Position,
                        Length = item.item.Length,
                        Text = item.item.Text
                    }).ToList()
            }).ToList();

            intentModel.Templates = intentModel.UserSays.Select(x => x.Data.GetTemplateString()).ToList();

            intentModel.Responses = dc.IntentResponses
                .Where(x => x.IntentId == intentModel.Id)
                .Select(x => x.Map<DmIntentResponse>())
                .ToList();

            intentModel.Responses.ForEach(response =>
            {
                // Load message
                response.Messages = dc.IntentResponseMessages.Where(x => x.IntentResponseId == response.Id)
                    .Select(x => x.Map< DmIntentResponseMessage>()).ToList();

                List<string> messageIds = response.Messages.Select(x => x.Id).ToList();
                var contents = dc.IntentResponseMessageContents.Where(x => messageIds.Contains(x.IntentResponseMessageId)).ToList();

                response.Messages.ForEach(message => {
                    message.Speech = contents.Where(x => x.IntentResponseMessageId == message.Id).Select(x => x.Content).ToList();
                });

                // Load parameters
                response.Parameters = dc.IntentResponseParameters.Where(x => x.IntentResponseId == response.Id)
                                    .Select(x => x.Map<DmIntentResponseParameter>()).ToList();

                List<string> parameterIds = response.Parameters.Select(x => x.Id).ToList();
                List<string> entityIds = response.Parameters.Where(x => !String.IsNullOrEmpty(x.EntityId)).Select(x => x.EntityId).ToList();
                var prompts = dc.IntentResponseParameterPrompts.Where(x => parameterIds.Contains(x.IntentResponseParameterId)).ToList();
                var entities = dc.Entities.Where(x => entityIds.Contains(x.Id)).ToList();

                response.Parameters.ForEach(parameter => {
                    parameter.Prompts = prompts.Where(x => x.IntentResponseParameterId == parameter.Id).Select(x => x.Text).ToList();
                    if (!String.IsNullOrEmpty(parameter.EntityId))
                    {
                        parameter.DataType = entities.Find(x => x.Id == parameter.EntityId).Name;
                    }
                });

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

            dc.Intents.Add(intentRecord);

            intentModel.Contexts.ForEach(context =>
            {
                dc.IntentInputContexts.Add(new IntentInputContexts
                {
                    Id = Guid.NewGuid().ToString(),
                    IntentId = intentModel.Id,
                    Name = context
                });
            });

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
            var intentRecord = dc.Intents.Find(intentModel.Id);
            intentRecord.Name = intentModel.Name;
            intentRecord.ModifiedDate = DateTime.UtcNow;

            // Remove all related data then create with same IntentId
            intentModel.UpdateInputContexts(dc);
            intentModel.UserSays.ForEach(expression => expression.Update(dc));
            intentModel.Responses.ForEach(response => response.Update(dc));
        }

        public static void UpdateInputContexts(this DmIntent intentModel, CoreDbContext dc)
        {
            dc.IntentInputContexts.RemoveRange(dc.IntentInputContexts.Where(x => x.IntentId == intentModel.Id));

            intentModel.Contexts.ForEach(context =>
            {
                dc.IntentInputContexts.Add(new IntentInputContexts
                {
                    Id = Guid.NewGuid().ToString(),
                    IntentId = intentModel.Id,
                    Name = context
                });
            });
        }
    }
}
