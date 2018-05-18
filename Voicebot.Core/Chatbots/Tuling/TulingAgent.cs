using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.Core.Chatbots.Tuling
{
    public class TulingAgent
    {
        public TulingResponse Request(TulingRequest req)
        {
            req.UserInfo = new TulingRequestUserInfo
            {
                ApiKey = Database.Configuration.GetSection("Tuling:ApiKey").Value,
                UserId = Database.Configuration.GetSection("Tuling:UserId").Value
            };

            var client = new RestClient($"{Database.Configuration.GetSection("Tuling:Url").Value}");

            var rest = new RestRequest("/openapi/api/v2", Method.POST);
            rest.RequestFormat = DataFormat.Json;
            string json = JsonConvert.SerializeObject(req,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            rest.AddParameter("application/json", json, ParameterType.RequestBody);

            var result = client.Execute(rest);

            return JsonConvert.DeserializeObject<TulingResponse>(result.Content);
        }
    }
}
