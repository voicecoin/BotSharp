using Core;
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
    public class CnDbPeidaRdf
    {
        public static List<Triple> QueryEntity(CoreDbContext Dc, string s)
        {
            NlpParseCache cache = Dc.Table<NlpParseCache>().FirstOrDefault(x => x.Parser == NlpEngine.CnDbPedia && x.Type == CacheType.Knowledge && x.Text == s);

            if (cache == null)
            {
                cache = new NlpParseCache();

                string content = RestHelper.GetSync("http://knowledgeworks.cn:20313/cndbpedia/api/entityAVP?entity=" + s);

                content = content.Replace("av pair", "Pairs");
                Dc.NlpParseCacheUpset(NlpEngine.CnDbPedia, s, CacheType.Knowledge, content);

                cache.ParsedJson = content;
            }

            //cache.ParsedJson = cache.ParsedJson.Replace("\"DESC\",", "\"简介\",");

            CnDbPediaResponse tags = JsonConvert.DeserializeObject<CnDbPediaResponse>(cache.ParsedJson);

            List<Triple> triples = tags.Pairs.Select(x => new Triple(s, x[0], x[1])).ToList();

            return triples;
        }

        private class CnDbPediaResponse
        {
            //[JsonProperty("av pair")]
            public List<List<String>> Pairs { get; set; }
        }
    }

    public class Triple
    {
        public Triple(string subject, string predict, string obj)
        {
            this.Subject = subject;
            this.Predicate = predict;
            this.Object = obj;
        }

        public string Subject { get; set; }
        public string Predicate { get; set; }
        public string Object { get; set; }
    }

    public interface INode { }
}