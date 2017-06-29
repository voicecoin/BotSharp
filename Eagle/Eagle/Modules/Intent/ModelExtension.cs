using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.Models;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Model.Extensions
{
    public static partial class ModelExtension
    {
        public static void Add(this IntentModel intentModel, DataContexts dc)
        {
            intentModel.Id = Guid.NewGuid().ToString();
            var intentRecord = intentModel.Map<Intents>();
            intentRecord.CreatedDate = DateTime.UtcNow;

            dc.Intents.Add(intentRecord);

            if(intentModel.Contexts != null)
            {
                intentModel.Contexts.ForEach(context => {
                    dc.IntentInputContexts.Add(new IntentInputContexts {
                        Id = Guid.NewGuid().ToString(),
                        IntentId = intentModel.Id,
                        Name = context
                    });
                });
            }

            if(intentModel.UserSays != null)
            {
                intentModel.UserSays.ForEach(userSay => {
                    userSay.IntentId = intentRecord.Id;
                    userSay.Add(dc);
                });
            }

            if(intentModel.Responses != null)
            {
                intentModel.Responses.ForEach(response => {
                    response.IntentId = intentRecord.Id;
                    response.Add(dc);
                });
            }
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

            if (intentModel.Contexts != null)
            {
                intentModel.Contexts.ForEach(context => {
                    dc.IntentInputContexts.Add(new IntentInputContexts
                    {
                        Id = Guid.NewGuid().ToString(),
                        IntentId = intentModel.Id,
                        Name = context
                    });
                });
            }
        }

        public static void UpdateExpressions(this IntentModel intentModel, DataContexts dc)
        {
            // Remove
            dc.IntentExpressions.RemoveRange();

            if (intentModel.UserSays != null)
            {
                intentModel.UserSays.ForEach(userSay => {
                    userSay.IntentId = intentModel.Id;
                    userSay.Add(dc);
                });
            }
        }

        public static void UpdateResponses(this IntentModel intentModel, DataContexts dc)
        {
            dc.IntentResponses.RemoveRange();

            if (intentModel.Responses != null)
            {
                intentModel.Responses.ForEach(response => {
                    response.IntentId = intentModel.Id;
                    response.Add(dc);
                });
            }
        }

        public static void Add(this IntentExpressionModel expressionModel, DataContexts dc)
        {
            var expressionRecord = expressionModel.Map<IntentExpressions>();
            expressionRecord.Id = Guid.NewGuid().ToString();
            expressionRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentExpressions.Add(expressionRecord);

            int pos = 0;
            expressionModel.Data.ForEach(item => {
                item.IntentExpressionId = expressionRecord.Id;
                pos = item.Add(dc, pos);
            });
        }

        public static int Add(this IntentExpressionItemModel expressionItemModel, DataContexts dc, int pos)
        {
            var entity = dc.Entities.FirstOrDefault(x => x.Name + "@" == expressionItemModel.Meta);

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

            if(responseModel.AffectedContexts != null)
            {
                responseModel.AffectedContexts.ForEach(context => {
                    context.IntentResponseId = responseRecord.Id;
                    context.Add(dc);
                });
            }

            if (responseModel.Messages != null)
            {
                responseModel.Messages.ForEach(message => {
                    message.IntentResponseId = responseRecord.Id;
                    message.Add(dc);
                });
            }

            if (responseModel.Parameters != null)
            {
                responseModel.Parameters.ForEach(parameter => {
                    parameter.IntentResponseId = responseRecord.Id;
                    parameter.Add(dc);
                });
            }
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

            if(responseParameterModel.Prompts != null)
            {
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
}
