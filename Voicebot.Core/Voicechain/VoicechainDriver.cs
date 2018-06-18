using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.Core.Voicechain
{
    public class VoicechainDriver
    {
        private Database dc;

        public VoicechainDriver(Database dc)
        {
            this.dc = dc;
        }

        public VoicechainResponse<ANameModel> GetAName(String name)
        {
            string domain = dc.Table<VnsTable>().FirstOrDefault(x => x.Name.ToLower() == name.ToLower())?.Domain;

            var client = new RestClient($"{Database.Configuration.GetSection("Voicechain:Host").Value}");

            var rest = new RestRequest(Database.Configuration.GetSection("Voicechain:Resource").Value, Method.GET);
            rest.RequestFormat = DataFormat.Json;
            rest.AddQueryParameter("name", domain);

            var result = client.Execute(rest);

            var aName = JsonConvert.DeserializeObject<VoicechainResponse<ANameModel>>(result.Content);

            aName.Data.Domain = domain;

            return aName;
        }
    }
}
