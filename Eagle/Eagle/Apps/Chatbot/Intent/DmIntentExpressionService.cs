using Eagle.Apps.Chatbot.DmServices;
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
            dc.IntentExpressionItems.RemoveRange(dc.IntentExpressionItems.Where(x => x.IntentExpressionId == intentExpression.Id));
            dc.IntentExpressions.Remove(dc.IntentExpressions.Find(intentExpression.Id));
            dc.SaveChanges();
        }

        public static void Add(this DmIntentExpression intentExpression, CoreDbContext dc)
        {
            var expressionRecord = intentExpression.MapByJsonString<IntentExpressions>();
            expressionRecord.CreatedDate = DateTime.UtcNow;
            expressionRecord.ModifiedDate = DateTime.UtcNow;
            dc.IntentExpressions.Add(expressionRecord);

            int pos = 0;
            intentExpression.Data.ForEach(item => {
                item.IntentExpressionId = expressionRecord.Id;
                pos = item.Add(dc, pos);
            });
        }
    }
}
