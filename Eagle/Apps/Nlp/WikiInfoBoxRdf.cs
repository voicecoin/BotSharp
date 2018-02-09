using Core;
using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Utility;

namespace Apps.Nlp
{
    public class WikiInfoBoxRdf
    {
        public static List<Triple> QueryEntity(Database Dc, string s)
        {
            NlpParseCache cache = Dc.Table<NlpParseCache>().FirstOrDefault(x => x.Parser == NlpEngine.YayaAi && x.Type == CacheType.Knowledge && x.Text == s);

            if (cache == null)
            {
                cache = new NlpParseCache();

                string content = RestHelper.GetSync("http://ai.yaya.ai:8007/nlpir/wikinfobox/" + s);

                content = content.Replace("av pair", "Pairs");
                Dc.NlpParseCacheUpset(NlpEngine.YayaAi, s, CacheType.Knowledge, content);

                cache.ParsedJson = content;
            }

            //cache.ParsedJson = cache.ParsedJson.Replace("\"DESC\",", "\"简介\",");

            WikiInfoBoxResponse tags = JsonConvert.DeserializeObject<WikiInfoBoxResponse>(cache.ParsedJson);

            List<Triple> triples = tags.Pairs.Select(x => new Triple(s, x[0], x[1])).ToList();

            return triples;
        }

        private class WikiInfoBoxResponse
        {
            //[JsonProperty("av pair")]
            public List<List<String>> Pairs { get; set; }
        }
    }
}