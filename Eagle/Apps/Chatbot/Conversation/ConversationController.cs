using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core;
using Apps.Chatbot.DomainModels;
using Utility;
using Apps.Chatbot.DmServices;
using Apps.Chatbot.Agent;
using Enyim.Caching;

namespace Apps.Chatbot.Conversation
{
    public class ConversationController : CoreController
    {
        public ConversationController(IMemcachedClient memcachedClient)
        {
            dc.MemcachedClient = memcachedClient;
        }
            
        [HttpGet]
        public async Task<String> Text(DmAgentRequest analyzerModel)
        {
            analyzerModel.Log(MyLogLevel.DEBUG);
            // Yaya UserName: gh_0a3fe78f2d13, key: ce36fa6d0ec047248da3354519658734
            // Lingxihuagu UserName: gh_c96a6311ab6d, key: f8bc556e63364c5a8b4e37000d897704

            var agentRecord = dc.Table<AgentEntity>().First(x => x.ClientAccessToken == analyzerModel.ClientAccessToken);
            DmAgentRequest agentRequestModel = new DmAgentRequest { Agent = agentRecord, Text = analyzerModel.Text };

            var response = agentRequestModel.TextRequest(dc, Configuration.GetSection("NlpApi:NlpirUrl").Value);

            if (response == null || String.IsNullOrEmpty(response.Text))
            {
                var result = await RestHelper.Rest<TulingResponse>("http://www.tuling123.com/openapi/api",
                    new
                    {
                        userid = analyzerModel.SessionId,
                        key = "ce36fa6d0ec047248da3354519658734",
                        info = analyzerModel.Text
                    });

                result.Log(MyLogLevel.DEBUG);

                return result.Text;
            }
            else
            {
                response.Log(MyLogLevel.DEBUG);

                return response.Text;
            }

        }
    }

    public class TulingResponse
    {
        public int Code { get; set; }
        public string Text { get; set; }
    }
}