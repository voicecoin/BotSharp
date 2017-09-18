using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Intent;
using Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Apps.Chatbot.Intent
{
    public static class DmIntentExpressionService
    {
        public static void Update(this DomainModel<IntentExpressionEntity> intentExpression)
        {
            CoreDbContext dc = intentExpression.Dc;
            // Remove Items first
            intentExpression.Delete();
            // Add back
            intentExpression.AddEntity();
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

                intentExpression.Entity.DataJson = JsonConvert.SerializeObject(intentExpression.Entity.Data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
        }
    }
}
