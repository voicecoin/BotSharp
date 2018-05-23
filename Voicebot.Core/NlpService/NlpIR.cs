using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.Core.NlpService
{
    public class NlpIR : INlpEngine
    {
        public List<NlpEntity> Ner(string text)
        {
            var client = new RestClient($"{Database.Configuration.GetSection("Nlpir:Url").Value}");

            var rest = new RestRequest($"/nlpir/wordsplit/{text}", Method.GET);

            var result = client.Execute(rest);

            var jObject = JObject.FromObject(JsonConvert.DeserializeObject(result.Content));

            var nlpIrResult = jObject.ToObject<NlpIrResult>();

            return nlpIrResult.WordSplit
                .Select(x => new NlpEntity { Name = x.Entity.Split(':').Last(), Value = x.Word })
                .ToList();
        }
    }


    public class NlpIrResult
    {
        public List<NlpIrWord> WordSplit { get; set; }
    }

    public class NlpIrWord
    {
        public string Word { get; set; }

        public string Entity { get; set; }
    }
}
