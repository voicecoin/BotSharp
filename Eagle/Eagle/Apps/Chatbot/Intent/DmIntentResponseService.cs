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
            var response = dc.Chatbot_IntentResponses.Find(responseModel.Id);
            if(response == null)
            {
                
            }
            response.Action = responseModel.Action;
            response.AffectedContexts = responseModel.AffectedContexts.ToArray();

            dc.Chatbot_IntentResponseMessages.RemoveRange(dc.Chatbot_IntentResponseMessages.Where(x => x.IntentResponseId == responseModel.Id));
            dc.SaveChanges();

            responseModel.Messages.ForEach(message => {
                dc.Chatbot_IntentResponseMessages.Add(message.Map<IntentResponseMessages>());
            });

            dc.Chatbot_IntentResponseParameters.RemoveRange(dc.Chatbot_IntentResponseParameters.Where(x => x.IntentResponseId == responseModel.Id));
            dc.SaveChanges();

            responseModel.Parameters.ForEach(parameter => {
                parameter.IntentResponseId = responseModel.Id;
                dc.Chatbot_IntentResponseParameters.Add(parameter.Map<IntentResponseParameters>());
            });
        }

        public static void Delete(this DmIntentResponse responseModel, CoreDbContext dc)
        {
            // Remove Items first
            responseModel.Parameters.ForEach(parameter => {

            });

            responseModel.Messages.ForEach(message => {

            });

            dc.Chatbot_IntentResponses.Remove(dc.Chatbot_IntentResponses.Find(responseModel.Id));

            dc.SaveChanges();
        }

        public static void Add(this DmIntentResponse responseModel, CoreDbContext dc)
        {
            var responseRecord = responseModel.Map<IntentResponses>();
            responseRecord.Id = Guid.NewGuid().ToString();
            responseRecord.CreatedDate = DateTime.UtcNow;
            responseRecord.AffectedContexts = responseModel.AffectedContexts.ToArray();
            dc.Chatbot_IntentResponses.Add(responseRecord);

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

        public static void Add(this DmIntentResponseMessage responseMessageModel, CoreDbContext dc)
        {
            var responseMessageRecord = responseMessageModel.Map<IntentResponseMessages>();
            responseMessageRecord.Id = Guid.NewGuid().ToString();
            responseMessageRecord.CreatedDate = DateTime.UtcNow;
            dc.Chatbot_IntentResponseMessages.Add(responseMessageRecord);
        }

        public static void Add(this DmIntentResponseParameter responseParameterModel, CoreDbContext dc)
        {
            var responseParameterRecord = responseParameterModel.Map<IntentResponseParameters>();
            responseParameterRecord.Id = Guid.NewGuid().ToString();
            responseParameterRecord.CreatedDate = DateTime.UtcNow;
            responseParameterRecord.Prompts = responseParameterModel.Prompts.ToArray();
            dc.Chatbot_IntentResponseParameters.Add(responseParameterRecord);
        }
    }
}
