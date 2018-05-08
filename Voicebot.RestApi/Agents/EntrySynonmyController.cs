using BotSharp.Core.Entities;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    public class EntrySynonmyController : CoreController
    {
        [HttpPost("{entryId}")]
        public EntityEntrySynonym CreateEntrySynonym([FromRoute] string entryId, [FromBody] EntityEntrySynonym synonym)
        {
            dc.DbTran(() =>
            {
                synonym.EntityEntryId = entryId;
                dc.Table<EntityEntrySynonym>().Add(synonym);
            });

            return synonym;
        }

        [HttpDelete("{entryId}/{synonym}")]
        public String DeleteEntrySynonym([FromRoute] string entryId, [FromRoute] string synonym)
        {
            dc.DbTran(() =>
            {
                var existedSynonym = dc.Table<EntityEntrySynonym>()
                    .FirstOrDefault(x => x.EntityEntryId == entryId && x.Synonym == synonym);

                if (existedSynonym != null)
                {
                    dc.Table<EntityEntrySynonym>().Remove(existedSynonym);
                }
            });

            return synonym;
        }
    }
}
