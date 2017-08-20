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
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Apps.Chatbot.Conversation
{
    public class ConversationController : CoreController
    {
        public ConversationController(IMemcachedClient memcachedClient)
        {
            dc.MemcachedClient = memcachedClient;
        }

        [HttpGet("{conversationId}/Reset")]
        public void Reset(string conversationId)
        {
            dc.Transaction<IDbRecord4SqlServer>(delegate {

                var conversation = dc.Table<ConversationEntity>().Find(conversationId);
                conversation.Keyword = String.Empty;

                dc.Table<ConversationParameterEntity>()
                    .RemoveRange(dc.Table<ConversationParameterEntity>()
                                    .Where(x => x.ConversationId == conversationId)
                                    .ToList());

                dc.Table<ConversationMessageEntity>()
                    .RemoveRange(dc.Table<ConversationMessageEntity>()
                                    .Where(x => x.ConversationId == conversationId)
                                    .ToList());

            });
        }

        [HttpGet("{agentId}")]
        public String Init(string agentId)
        {
            var conversationId = Guid.Empty.ToString();

            dc.CurrentUser = GetCurrentUser();
            var conversation = dc.Table<ConversationEntity>().FirstOrDefault(x => x.AgentId == agentId && x.CreatedUserId == dc.CurrentUser.Id);
            if (conversation == null)
            {
                dc.Transaction<IDbRecord4SqlServer>(delegate
                {
                    var dm = new DomainModel<ConversationEntity>(dc, new ConversationEntity
                    {
                        AgentId = agentId
                    });

                    dm.AddEntity();

                    conversationId = dm.Entity.Id;
                });
            }
            else
            {
                conversationId = conversation.Id;
            }

            return conversationId;
        }
        
        public async Task<String> Text(DmAgentRequest analyzerModel)
        {
            // analyzerModel.Log(MyLogLevel.DEBUG);
            // Yaya UserName: gh_0a3fe78f2d13, key: ce36fa6d0ec047248da3354519658734
            // Lingxihuagu UserName: gh_c96a6311ab6d, key: f8bc556e63364c5a8b4e37000d897704
            dc.CurrentUser = GetCurrentUser();
            var timeStart = DateTime.UtcNow;

            var agentRecord = dc.Table<AgentEntity>().First(x => x.ClientAccessToken == analyzerModel.ClientAccessToken);
            DmAgentRequest agentRequestModel = new DmAgentRequest { Agent = agentRecord, Text = analyzerModel.Text, ConversationId = analyzerModel.ConversationId };

            var response = agentRequestModel.TextRequest(dc, Configuration.GetSection("NlpApi:NlpirUrl").Value);

            if (response == null || String.IsNullOrEmpty(response.Text))
            {
                var url = Configuration.GetSection("NlpApi:TulingUrl").Value;
                var key = Configuration.GetSection("NlpApi:TulingKey").Value;

                var result = await RestHelper.Rest<TulingResponse>(url,
                    new
                    {
                        userid = analyzerModel.ConversationId,
                        key = key,
                        info = analyzerModel.Text
                    });

                result.ResponseTime = (DateTime.UtcNow - timeStart).Milliseconds;
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
        public int ResponseTime { get; set; }
    }
}