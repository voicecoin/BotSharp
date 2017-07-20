using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Intent;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Apps.Chatbot
{
    public static class DmIntentResponseService
    {
        public static void Update(this DmIntentResponse responseModel, CoreDbContext dc)
        {
            var response = dc.Table<IntentResponseEntity>().Find(responseModel.Id);
            if(response == null)
            {
                
            }
            response.Action = responseModel.Action;
            response.AffectedContexts = responseModel.AffectedContexts.ToArray();

            dc.Table<IntentResponseMessageEntity>().RemoveRange(dc.Table<IntentResponseMessageEntity>().Where(x => x.IntentResponseId == responseModel.Id));
            dc.SaveChanges();

            responseModel.Messages.ForEach(message => {
                dc.Table<IntentResponseMessageEntity>().Add(message.Map<IntentResponseMessageEntity>());
            });

            dc.Table<IntentResponseParameterEntity>().RemoveRange(dc.Table<IntentResponseParameterEntity>().Where(x => x.IntentResponseId == responseModel.Id));
            dc.SaveChanges();

            responseModel.Parameters.ForEach(parameter => {
                parameter.IntentResponseId = responseModel.Id;
                dc.Table<IntentResponseParameterEntity>().Add(parameter.Map<IntentResponseParameterEntity>());
            });
        }

        public static void Delete(this DmIntentResponse responseModel, CoreDbContext dc)
        {
            // Remove Items first
            responseModel.Parameters.ForEach(parameter => {

            });

            responseModel.Messages.ForEach(message => {

            });

            dc.Table<IntentResponseEntity>().Remove(dc.Table<IntentResponseEntity>().Find(responseModel.Id));

            dc.SaveChanges();
        }

        public static void Add(this DmIntentResponse responseModel, CoreDbContext dc)
        {
            var responseRecord = responseModel.Map<IntentResponseEntity>();
            responseRecord.Id = Guid.NewGuid().ToString();
            responseRecord.CreatedDate = DateTime.UtcNow;
            responseRecord.CreatedUserId = dc.CurrentUser.Id;
            responseRecord.ModifiedDate = DateTime.UtcNow;
            responseRecord.ModifiedUserId = dc.CurrentUser.Id;
            responseRecord.AffectedContexts = responseModel.AffectedContexts.ToArray();
            dc.Table<IntentResponseEntity>().Add(responseRecord);

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
            var responseMessageRecord = responseMessageModel.Map<IntentResponseMessageEntity>();
            responseMessageRecord.Id = Guid.NewGuid().ToString();
            responseMessageRecord.CreatedDate = DateTime.UtcNow;
            responseMessageRecord.CreatedUserId = dc.CurrentUser.Id;
            responseMessageRecord.ModifiedDate = DateTime.UtcNow;
            responseMessageRecord.ModifiedUserId = dc.CurrentUser.Id;
            dc.Table<IntentResponseMessageEntity>().Add(responseMessageRecord);
        }

        public static void Add(this DmIntentResponseParameter responseParameterModel, CoreDbContext dc)
        {
            var responseParameterRecord = responseParameterModel.Map<IntentResponseParameterEntity>();
            responseParameterRecord.Id = Guid.NewGuid().ToString();
            responseParameterRecord.CreatedDate = DateTime.UtcNow;
            responseParameterRecord.CreatedUserId = dc.CurrentUser.Id;
            responseParameterRecord.ModifiedDate = DateTime.UtcNow;
            responseParameterRecord.ModifiedUserId = dc.CurrentUser.Id;

            responseParameterRecord.Prompts = responseParameterModel.Prompts.ToArray();
            dc.Table<IntentResponseParameterEntity>().Add(responseParameterRecord);
        }
    }
}
