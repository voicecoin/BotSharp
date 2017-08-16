using Apps.Chatbot_ConversationParameters.DomainModels;
using Apps.Chatbot_ConversationParameters.Intent;
using Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Apps.Chatbot_ConversationParameters
{
    public static class DmIntentExpressionService
    {
        public static void Update(this DomainModel<IntentExpressionEntity> intentExpression)
        {
            CoreDbContext dc = intentExpression.Dc;
            // Remove Items first
            intentExpression.Delete();
            // Add back
            intentExpression.Add();
        }

        public static void Delete(this DomainModel<IntentExpressionEntity> intentExpression)
        {
            CoreDbContext dc = intentExpression.Dc;
            // Remove Items first
            if (String.IsNullOrEmpty(intentExpression.Entity.Id)) return;

            dc.Table<IntentExpressionEntity>().Remove(dc.Table<IntentExpressionEntity>().Find(intentExpression.Entity.Id));
            dc.SaveChanges();
        }

        public static void Add(this DomainModel<IntentExpressionEntity> intentExpression)
        {
            if (!intentExpression.AddEntity()) return;

            int pos = 0;

            if(intentExpression.Entity.Data != null)
            {
                intentExpression.Entity.Data.ForEach(item => {
                    item.Length = item.Text.Length;
                    item.Position = pos;
                    pos += item.Text.Length;
                });

                intentExpression.Entity.DataJson = JsonConvert.SerializeObject(intentExpression.Entity.Data);
            }
        }
    }
}
