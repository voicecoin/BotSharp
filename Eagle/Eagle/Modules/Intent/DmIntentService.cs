using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.DomainModels;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DmServices
{
    public static partial class DmIntentService
    {
        public static void Load(this DmIntent intentModel, DataContexts dc)
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
                Template = intentExpressionItems.Where(item => item.item.IntentExpressionId == expression.Id).Select(x => x.item.Map<DmIntentExpressionItem>()).GetTemplateString(),
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

            intentModel.Templates = intentModel.UserSays.Where(x => !String.IsNullOrEmpty(x.Template)).Select(x => x.Template).ToList();

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


        public static void Add(this DmIntent intentModel, DataContexts dc)
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

        public static void Update(this DmIntent intentModel, DataContexts dc)
        {
            var intentRecord = dc.Intents.Find(intentModel.Id);
            intentRecord.Name = intentModel.Name;
            intentRecord.ModifiedDate = DateTime.UtcNow;

            // Remove all related data then create with same IntentId
            /*intentModel.UpdateInputContexts(dc);
            intentModel.UpdateExpressions(dc);
            intentModel.UpdateResponses(dc);*/
        }

        public static void UpdateInputContexts(this DmIntent intentModel, DataContexts dc)
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

        public static void UpdateExpressions(this DmIntent intentModel, DataContexts dc)
        {
            intentModel.UserSays.ForEach(expression => UpdateExpression(expression, dc));
        }

        public static void UpdateExpression(this DmIntentExpression intentExpression, DataContexts dc)
        {
            // Remove Items first

            var intentExpressionRecord = dc.IntentExpressions.Find(intentExpression.Id);
            dc.IntentExpressions.Remove(intentExpressionRecord);

            /*intentModel.UserSays.ForEach(userSay =>
            {
                userSay.IntentId = intentModel.Id;
                userSay.Add(dc);
            });*/
            
        }

        public static void UpdateResponses(this DmIntent intentModel, DataContexts dc)
        {
            dc.IntentResponses.RemoveRange();

            intentModel.Responses.ForEach(response =>
            {
                response.IntentId = intentModel.Id;
                response.Add(dc);
            });
        }

        public static void Add(this DmIntentExpression expressionModel, DataContexts dc)
        {
            var expressionRecord = expressionModel.Map<IntentExpressions>();
            dc.IntentExpressions.Add(expressionRecord);

            int pos = 0;
            expressionModel.Data.ForEach(item => {
                item.IntentExpressionId = expressionRecord.Id;
                pos = item.Add(dc, pos);
            });
        }

        public static int Add(this DmIntentExpressionItem expressionItemModel, DataContexts dc, int pos)
        {
            var expressionItemRecord = expressionItemModel.Map<IntentExpressionItems>();
            expressionItemRecord.Id = Guid.NewGuid().ToString();
            expressionItemRecord.Length = expressionItemRecord.Text.Length;
            expressionItemRecord.Position = pos;
            expressionItemRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentExpressionItems.Add(expressionItemRecord);

            return pos + expressionItemRecord.Length;
        }

        public static void Add(this DmIntentResponse responseModel, DataContexts dc)
        {
            var responseRecord = responseModel.Map<IntentResponses>();
            responseRecord.Id = Guid.NewGuid().ToString();
            responseRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentResponses.Add(responseRecord);

            responseModel.AffectedContexts.ForEach(context =>
            {
                context.IntentResponseId = responseRecord.Id;
                context.Add(dc);
            });

            responseModel.Messages.ForEach(message =>
            {
                message.IntentResponseId = responseRecord.Id;
                message.Add(dc);
            });

            responseModel.Parameters.ForEach(parameter =>
            {
                parameter.IntentResponseId = responseRecord.Id;
                parameter.Add(dc);
            });
        }

        public static void Add(this DmIntentResponseContext responseContextModel, DataContexts dc)
        {
            var responseContextRecord = responseContextModel.Map<IntentResponseContexts>();
            responseContextRecord.Id = Guid.NewGuid().ToString();
            responseContextRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentResponseContexts.Add(responseContextRecord);
        }

        public static void Add(this DmIntentResponseMessage responseMessageModel, DataContexts dc)
        {
            var responseMessageRecord = responseMessageModel.Map<IntentResponseMessages>();
            responseMessageRecord.Id = Guid.NewGuid().ToString();
            responseMessageRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentResponseMessages.Add(responseMessageRecord);

            responseMessageModel.Speech.ForEach(speech =>
            {
                dc.IntentResponseMessageContents.Add(new IntentResponseMessageContents
                {
                    IntentResponseMessageId = responseMessageRecord.Id,
                    Content = speech
                });
            });
        }

        public static void Add(this DmIntentResponseParameter responseParameterModel, DataContexts dc)
        {
            var responseParameterRecord = responseParameterModel.Map<IntentResponseParameters>();
            responseParameterRecord.Id = Guid.NewGuid().ToString();
            responseParameterRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentResponseParameters.Add(responseParameterRecord);

            responseParameterModel.Prompts.ForEach(prompt =>
            {
                dc.IntentResponseParameterPrompts.Add(new IntentResponseParameterPrompts
                {
                    IntentResponseParameterId = responseParameterRecord.Id,
                    Text = prompt
                });
            });
        }
    }
}
