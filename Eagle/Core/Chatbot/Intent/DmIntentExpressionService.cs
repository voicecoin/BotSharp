using Apps.Chatbot.DomainModels;
using Core.DataContexts;
using Core.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Apps.Chatbot
{
    public static class DmIntentExpressionService
    {
        public static void Update(this DmIntentExpression intentExpression, CoreDbContext dc)
        {
            // Remove Items first
            intentExpression.Delete(dc);
            // Add back
            intentExpression.Add(dc);
        }

        public static void Delete(this DmIntentExpression intentExpression, CoreDbContext dc)
        {
            // Remove Items first
            if (String.IsNullOrEmpty(intentExpression.Id)) return;

            dc.Chatbot_IntentExpressions.Remove(dc.Chatbot_IntentExpressions.Find(intentExpression.Id));
            dc.SaveChanges();
        }

        public static void Add(this DmIntentExpression intentExpression, CoreDbContext dc)
        {
            if (String.IsNullOrEmpty(intentExpression.Id))
            {
                intentExpression.Id = Guid.NewGuid().ToString();
            }

            var expressionRecord = intentExpression.MapByJsonString<IntentExpressions>();
            expressionRecord.CreatedDate = DateTime.UtcNow;
            expressionRecord.ModifiedDate = DateTime.UtcNow;

            int pos = 0;
            intentExpression.Data.ForEach(item => {
                item.Length = item.Text.Length;
                item.Position = pos;
                pos += item.Text.Length;
            });
            expressionRecord.Items = intentExpression.Data.ToArray();

            dc.Chatbot_IntentExpressions.Add(expressionRecord);
            dc.SaveChanges();
        }
    }
}
