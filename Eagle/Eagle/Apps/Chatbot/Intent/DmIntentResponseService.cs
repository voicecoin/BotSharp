using Eagle.Apps.Chatbot.DomainModels;
using Eagle.DataContexts;
using Eagle.DbTables;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Apps.Chatbot
{
    public static class DmIntentResponseService
    {
        public static void Update(this DmIntentResponse responseModel, CoreDbContext dc)
        {
            // Remove Items first
            responseModel.Delete(dc);
            // Add back
            responseModel.Add(dc);
        }

        public static void Delete(this DmIntentResponse responseModel, CoreDbContext dc)
        {
            // Remove Items first
            dc.IntentResponseContexts.RemoveRange(dc.IntentResponseContexts.Where(x => x.IntentResponseId == responseModel.Id));

            responseModel.Parameters.ForEach(parameter => {

            });

            responseModel.Messages.ForEach(message => {

            });

            dc.IntentResponses.Remove(dc.IntentResponses.Find(responseModel.Id));

            dc.SaveChanges();
        }

        public static void Add(this DmIntentResponse responseModel, CoreDbContext dc)
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

        public static void Add(this DmIntentResponseContext responseContextModel, CoreDbContext dc)
        {
            var responseContextRecord = responseContextModel.Map<IntentResponseContexts>();
            responseContextRecord.Id = Guid.NewGuid().ToString();
            responseContextRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentResponseContexts.Add(responseContextRecord);
        }

        public static void Add(this DmIntentResponseMessage responseMessageModel, CoreDbContext dc)
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

        public static void Add(this DmIntentResponseParameter responseParameterModel, CoreDbContext dc)
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
