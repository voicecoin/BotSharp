using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Intent;
using Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Apps.Chatbot
{
    public static class DmIntentResponseService
    {
        public static void Update(this DomainModel<IntentResponseEntity> responseModel)
        {
            CoreDbContext dc = responseModel.Dc;

            var response = dc.Table<IntentResponseEntity>().Find(responseModel.Entity.Id);
            if(response == null)
            {
                
            }

            response.Action = responseModel.Entity.Action;
            response.AffectedContextsJson = JsonConvert.SerializeObject(responseModel.Entity.AffectedContexts);

            dc.Table<IntentResponseMessageEntity>().RemoveRange(dc.Table<IntentResponseMessageEntity>().Where(x => x.IntentResponseId == responseModel.Entity.Id));
            dc.SaveChanges();

            responseModel.Entity.Messages.ForEach(message => {
                dc.Table<IntentResponseMessageEntity>().Add(message.Map<IntentResponseMessageEntity>());
            });

            dc.Table<IntentResponseParameterEntity>().RemoveRange(dc.Table<IntentResponseParameterEntity>().Where(x => x.IntentResponseId == responseModel.Entity.Id));
            dc.SaveChanges();

            responseModel.Entity.Parameters.ForEach(parameter => {
                parameter.IntentResponseId = responseModel.Entity.Id;
                dc.Table<IntentResponseParameterEntity>().Add(parameter.Map<IntentResponseParameterEntity>());
            });
        }

        public static void Delete(this DomainModel<IntentResponseEntity> responseModel, CoreDbContext dc)
        {
            // Remove Items first
            responseModel.Entity.Parameters.ForEach(parameter => {

            });

            responseModel.Entity.Messages.ForEach(message => {

            });

            dc.Table<IntentResponseEntity>().Remove(dc.Table<IntentResponseEntity>().Find(responseModel.Entity.Id));

            dc.SaveChanges();
        }

        public static void Add(this DomainModel<IntentResponseEntity> responseModel)
        {
            if (!responseModel.AddEntity()) return;

            if(responseModel.Entity.AffectedContexts != null)
            {
                responseModel.Entity.AffectedContextsJson = JsonConvert.SerializeObject(responseModel.Entity.AffectedContexts);
            }
            
            CoreDbContext dc = responseModel.Dc;

            responseModel.Entity.Messages.ForEach(message =>
            {
                message.IntentResponseId = responseModel.Entity.Id;
                if(message.Speeches != null)
                {
                    message.SpeechesJson = JsonConvert.SerializeObject(message.Speeches);
                }
                
                var dm = new DomainModel<IntentResponseMessageEntity>(dc, message);
                dm.AddEntity();
            });

            responseModel.Entity.Parameters.ForEach(parameter =>
            {
                parameter.IntentResponseId = responseModel.Entity.Id;
                if(parameter.Prompts != null)
                {
                    parameter.PromptsJson = JsonConvert.SerializeObject(parameter.Prompts);
                }
                
                var dm = new DomainModel<IntentResponseParameterEntity>(dc, parameter);
                dm.AddEntity();
            });
        }
    }
}
