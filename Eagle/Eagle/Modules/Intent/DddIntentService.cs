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
    public static partial class DddIntentService
    {
        public static void Load(this IntentModel intentModel, DataContexts dc)
        {
            var intentExpressions = dc.IntentExpressions.Where(x => x.IntentId == intentModel.Id).ToList();
            var intentExpressionItems = (from item in dc.IntentExpressionItems
                                         from entity in dc.Entities.Where(x => item.EntityId == x.Id).DefaultIfEmpty()
                                         where intentExpressions.Select(expression => expression.Id).Contains(item.IntentExpressionId)
                                         orderby item.Position
                                         select new { item, entity }).ToList();

            intentModel.UserSays = intentExpressions.Select(expression => new IntentExpressionModel
            {
                Id = expression.Id,
                Data = intentExpressionItems.Where(item => item.item.IntentExpressionId == expression.Id)
                    .Select(item => new IntentExpressionItemModel
                    {
                        Text = item.item.Text,
                        Meta = item.entity == null ? null : $"@{item.entity?.Name}",
                        Alias = item.entity?.Name
                    }).ToList()
            }).ToList();

            intentModel.Templates = intentExpressions.Select(x => x.Template).ToList();

            intentModel.Responses = dc.IntentResponses
                .Where(x => x.IntentId == intentModel.Id)
                .Select(x => x.Map<IntentResponseModel>())
                .ToList();

            intentModel.Responses.ForEach(response =>
            {
                response.Messages = dc.IntentResponseMessages.Where(x => x.IntentResponseId == response.Id)
                    .Select(x => x.Map< IntentResponseMessageModel>()).ToList();
            });
        }


        public static void Add(this IntentModel intentModel, DataContexts dc)
        {
            intentModel.Id = Guid.NewGuid().ToString();
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

        public static void Update(this IntentModel intentModel, DataContexts dc)
        {
            var intentRecord = dc.Intents.Find(intentModel.Id);
            intentRecord.Name = intentModel.Name;
            intentRecord.ModifiedDate = DateTime.UtcNow;

            // Remove all related data then create with same IntentId
            intentModel.UpdateInputContexts(dc);
            intentModel.UpdateExpressions(dc);
            intentModel.UpdateResponses(dc);
        }

        public static void UpdateInputContexts(this IntentModel intentModel, DataContexts dc)
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

        public static void UpdateExpressions(this IntentModel intentModel, DataContexts dc)
        {
            // Remove
            dc.IntentExpressions.RemoveRange();

            intentModel.UserSays.ForEach(userSay =>
            {
                userSay.IntentId = intentModel.Id;
                userSay.Add(dc);
            });
        }

        public static void UpdateResponses(this IntentModel intentModel, DataContexts dc)
        {
            dc.IntentResponses.RemoveRange();

            intentModel.Responses.ForEach(response =>
            {
                response.IntentId = intentModel.Id;
                response.Add(dc);
            });
        }

        public static void Add(this IntentExpressionModel expressionModel, DataContexts dc)
        {
            var expressionRecord = expressionModel.Map<IntentExpressions>();
            expressionRecord.Template = String.Concat(expressionModel.Data.Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta).ToArray());
            dc.IntentExpressions.Add(expressionRecord);

            int pos = 0;
            expressionModel.Data.ForEach(item => {
                item.IntentExpressionId = expressionRecord.Id;
                pos = item.Add(dc, pos);
            });
        }

        public static int Add(this IntentExpressionItemModel expressionItemModel, DataContexts dc, int pos)
        {
            var entity = dc.Entities.FirstOrDefault(x => "@" + x.Name == expressionItemModel.Meta);

            var expressionItemRecord = expressionItemModel.Map<IntentExpressionItems>();
            expressionItemRecord.Id = Guid.NewGuid().ToString();
            expressionItemRecord.EntityId = entity?.Id;
            expressionItemRecord.Length = expressionItemRecord.Text.Length;
            expressionItemRecord.Position = pos;
            expressionItemRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentExpressionItems.Add(expressionItemRecord);

            return pos + expressionItemRecord.Length;
        }

        public static void Add(this IntentResponseModel responseModel, DataContexts dc)
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

        public static void Add(this IntentResponseContextModel responseContextModel, DataContexts dc)
        {
            var responseContextRecord = responseContextModel.Map<IntentResponseContexts>();
            responseContextRecord.Id = Guid.NewGuid().ToString();
            responseContextRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentResponseContexts.Add(responseContextRecord);
        }

        public static void Add(this IntentResponseMessageModel responseMessageModel, DataContexts dc)
        {
            var responseMessageRecord = responseMessageModel.Map<IntentResponseMessages>();
            responseMessageRecord.Id = Guid.NewGuid().ToString();
            responseMessageRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentResponseMessages.Add(responseMessageRecord);
        }

        public static void Add(this IntentResponseParameterModel responseParameterModel, DataContexts dc)
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
