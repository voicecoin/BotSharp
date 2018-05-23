using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.Core.NlpService
{
    public class BosonNlp : INlpEngine
    {
        public List<NlpEntity> Ner(string text)
        {
            var client = new RestClient($"{Database.Configuration.GetSection("BosonNlp:Url").Value}");
            var token = Database.Configuration.GetSection("BosonNlp:Token").Value;

            var rest = new RestRequest("/ner/analysis", Method.POST);
            rest.RequestFormat = DataFormat.Json;
            string json = JsonConvert.SerializeObject(new string[] { text },
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            rest.AddParameter("application/json", json, ParameterType.RequestBody);
            rest.AddHeader("X-Token", token);

            var result = client.Execute(rest);

            var data = JObject.FromObject(JsonConvert.DeserializeObject(result.Content));

            return null;
        }
    }
}
