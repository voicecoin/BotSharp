using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core;
using Apps.Chatbot.DomainModels;
using Utility;
using Apps.Chatbot.DmServices;
using Apps.Chatbot.Agent;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Collections.Generic;

namespace Apps.Chatbot.Conversation
{
    public class ConversationController : CoreController
    {
        [HttpGet("{conversationId}/Reset")]
        public void Reset(string conversationId)
        {
            dc.Transaction<IDbRecord4Core>(delegate {

                var conversation = dc.Table<ConversationEntity>().Find(conversationId);

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
                dc.Transaction<IDbRecord4Core>(delegate
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

        [AllowAnonymous]
        [HttpGet("Test")]
        public async Task<String> Test()
        {
            var agentRecord = dc.Table<AgentEntity>().First(x => x.ClientAccessToken == "8084658aed844e3a985bca7b6c8cf0d3");
            DmAgentRequest agentRequestModel = new DmAgentRequest { Agent = agentRecord, ConversationId = Guid.NewGuid().ToString() };
            StringBuilder contents = new StringBuilder();

            List<String> questions = new List<string>() {
                "张学友身高"
            };
            List<String> answers = new List<string>();

            foreach (String question in questions)
            {
                agentRequestModel.Text = question;
                DmAgentResponse response = agentRequestModel.TextRequest(dc);
                answers.Add(response.Text);
            }

            // return text
            for (int i = 0; i < questions.Count; i++)
            {
                contents.AppendLine($"User: {questions[i]}");
                contents.AppendLine($" Bot: {answers[i]}");
                //contents.AppendLine();
            }

            return contents.ToString();
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
            DmAgentResponse response;

            response = agentRequestModel.TextRequest(dc);

            if (response == null || String.IsNullOrEmpty(response.Text))
            {
                // 是否转向闲聊机器人
                if (dc.Table<AgentSkillEntity>().Any(x => x.AgentId == agentRequestModel.Agent.Id && x.SkillId == "c3c498cf-3009-4791-9704-819a693577b7"))
                {
                    var url = CoreDbContext.Configuration.GetSection("NlpApi:TulingUrl").Value;
                    var key = CoreDbContext.Configuration.GetSection("NlpApi:TulingKey").Value;

                    var result = await RestHelper.Rest<TulingResponse>(url,
                        new
                        {
                            userid = analyzerModel.ConversationId,
                            key = key,
                            info = analyzerModel.Text
                        });

                    result.ResponseTime = (DateTime.UtcNow - timeStart).Milliseconds;
                    return result.Text;
                }
                else
                {
                    return "不明白你在说啥。";
                }
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