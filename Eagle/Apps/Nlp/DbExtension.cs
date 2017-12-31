using Core;
using Core.Interfaces;
using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apps.Nlp
{
    public static class DbExtension
    {
        public static void NlpParseCacheUpset(this CoreDbContext Dc, NlpEngine engine, string sentence, CacheType type, string json)
        {
            Dc.Transaction<IDbRecord>(delegate {
                Dc.Table<NlpParseCache>().RemoveRange(Dc.Table<NlpParseCache>().Where(x => x.Parser == engine && x.Type == type && x.Text == sentence));
                Dc.Table<NlpParseCache>().Add(new NlpParseCache
                {
                    Parser = engine,
                    Type = type,
                    Text = sentence,
                    ParsedJson = json
                });
            });
        }
    }
}