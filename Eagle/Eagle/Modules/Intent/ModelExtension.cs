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
            var entityRecord = intentModel.Map<Intents>();

            dc.Intents.Add(entityRecord);

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
                    userSay.IntentId = entityRecord.Id;
                    userSay.Add(dc);
                });
            }
        }

        public static void Add(this IntentExpressionModel expressionModel, DataContexts dc)
        {
            var expressionRecord = expressionModel.Map<IntentExpressions>();
            expressionRecord.Id = Guid.NewGuid().ToString();
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
            dc.IntentExpressionItems.Add(expressionItemRecord);

            return pos + expressionItemRecord.Length;
        }
    }
}
