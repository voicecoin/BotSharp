using Eagle.Apps.Chatbot.DomainModels;
using Eagle.DataContexts;
using Eagle.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Apps.Chatbot
{
    public static class DmIntentInputContextService
    {
        public static void UpdateInputContexts(this DmIntent intentModel, CoreDbContext dc)
        {
            dc.IntentInputContexts.RemoveRange(dc.IntentInputContexts.Where(x => x.IntentId == intentModel.Id));

            intentModel.Contexts.ForEach(context =>
            {
                dc.IntentInputContexts.Add(new IntentInputContexts
                {
                    Id = Guid.NewGuid().ToString(),
                    IntentId = intentModel.Id,
                    Name = context
                });
            });
        }
    }
}
