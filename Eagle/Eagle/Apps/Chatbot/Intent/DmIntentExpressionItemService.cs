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
    public static class DmIntentExpressionItemService
    {
        public static void Delete(this DmIntentExpressionItem expressionItemModel, CoreDbContext dc)
        {
            var intentExpressionRecord = dc.IntentExpressionItems.Find(expressionItemModel.Id);
            dc.IntentExpressionItems.Remove(intentExpressionRecord);
        }

        public static int Add(this DmIntentExpressionItem expressionItemModel, CoreDbContext dc, int pos)
        {
            var expressionItemRecord = expressionItemModel.Map<IntentExpressionItems>();
            expressionItemRecord.Id = Guid.NewGuid().ToString();
            expressionItemRecord.Length = expressionItemRecord.Text.Length;
            expressionItemRecord.Position = pos;
            expressionItemRecord.CreatedDate = DateTime.UtcNow;

            dc.IntentExpressionItems.Add(expressionItemRecord);

            return pos + expressionItemRecord.Length;
        }
    }
}
