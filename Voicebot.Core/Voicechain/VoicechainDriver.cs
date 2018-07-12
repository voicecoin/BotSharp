using DotNetToolkit;
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
            var aName = new VoicechainResponse<ANameModel>
            {
                Data = new ANameModel
                {
                    Address = "0.0.0.0",
                    Domain = name,
                    Time = DateTime.UtcNow.ToUnixTime()
                }
            };

            string domain = dc.Table<VnsTable>().FirstOrDefault(x => x.AgentId == name || x.Name.ToLower() == name.ToLower())?.Domain;

            var client = new RestClient($"{Database.Configuration.GetSection("Voicechain:Host").Value}");

            var rest = new RestRequest(Database.Configuration.GetSection("Voicechain:Resource").Value, Method.GET);
            rest.RequestFormat = DataFormat.Json;
            rest.AddQueryParameter("name", domain);

            var result = client.Execute(rest);

            try
            {
                var json = JsonConvert.DeserializeObject<VoicechainResponse<ANameModel>>(result.Content);
                if(json != null)
                {
                    aName = json;
                    aName.Data.Domain = domain;
                }
            }
            catch (Exception ex)
            {
                aName.Code = -1;
                aName.Message = result.Content;
                ex.Message.Log(LogLevel.ERROR);
            }

            return aName;
        }
    }
}
