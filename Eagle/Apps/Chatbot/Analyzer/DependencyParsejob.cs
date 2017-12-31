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
using EntityFrameworkCore.BootKit;

namespace Apps.Chatbot.Analyzer
{
    public class DependencyParseJob : ScheduleJobBase
    {
        public override Task Execute(IJobExecutionContext context)
        {
            var sentences = from intent in Dc.Table<IntentEntity>()
                            join expression in Dc.Table<IntentExpressionEntity>() on intent.Id equals expression.IntentId
                            where String.IsNullOrEmpty(expression.DataJson)
                            orderby expression.ModifiedDate
                            select new { ExpressionId = expression.Id, AgentId = intent.AgentId, Text = expression.Text };

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
                Dc.Transaction<IDbRecord>(delegate {
                    var expressionEntity = Dc.Table<IntentExpressionEntity>().Find(sentence.ExpressionId);
                    expressionEntity.DataJson = JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    expressionEntity.ModifiedDate = DateTime.UtcNow;
                    expressionEntity.ModifiedUserId = Constants.JobUserId;
                    expressionEntity.AllowOverrideData = true;
                });

            });
            
            return Task.CompletedTask;
        }
    }
}
