using Core.Scheduler;
using System;
using System.Collections.Generic;
using System.Text;
using Quartz;
using System.Threading.Tasks;
using Apps.Chatbot.Intent;
using System.Linq;
using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Agent;
using Apps.Chatbot.DmServices;
using Core.Interfaces;
using Newtonsoft.Json;
using Apps.Chatbot.Faq;

namespace Apps.Chatbot.Analyzer
{
    public class FaqPosJob : ScheduleJobBase
    {
        public override Task Execute(IJobExecutionContext context)
        {
            var sentences = from intent in Dc.Table<FaqEntity>()
                            where String.IsNullOrEmpty(intent.DataJson)
                            orderby intent.ModifiedDate
                            select new { ExpressionId = intent.Id, AgentId = intent.AgentId, Text = intent.Question };

            var result = sentences.Take(10).ToList();

            result.ForEach(sentence => {
                AgentEntity agent = new AgentEntity { Id = sentence.AgentId };
                DmAgentRequest agentRequestModel2 = new DmAgentRequest { Agent = agent, Text = sentence.Text };

                var data = agentRequestModel2.PosTagger(Dc).Select(x => new DmIntentExpressionItem
                {
                    Text = x.Text,
                    Alias = x.Alias,
                    Meta = x.Meta,
                    Position = x.Position,
                    Length = x.Length,
                    Color = x.Color,
                    Value = x.Value
                }).OrderBy(x => x.Position).ToList();

                var userSay = new IntentExpressionEntity() { Data = data };

                // Save to database
                Dc.Transaction<IDbRecord4Core>(delegate {
                    var expressionEntity = Dc.Table<FaqEntity>().Find(sentence.ExpressionId);
                    expressionEntity.DataJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    expressionEntity.ModifiedDate = DateTime.UtcNow;
                    expressionEntity.ModifiedUserId = Constants.JobUserId;
                });

            });
            
            return null;
        }
    }
}
