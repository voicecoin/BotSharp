using Apps.Chatbot.Agent;
using Apps.Chatbot.Conversation;
using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Intent;
using Core;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using MWL.DocumentResolver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Apps.Chatbot.DmServices
{
    public static class DmAgentService
    {
        public static DmAgentResponse TextRequest(this DmAgentRequest agentRequestModel, CoreDbContext dc)
        {
            // Remove Stop word.
            agentRequestModel.Text = agentRequestModel.Text.Replace("？", "");
            agentRequestModel.Text = agentRequestModel.Text.Replace("?", "");

            // Get 机器人技能
            List<String> allyIds = dc.Table<AgentSkillEntity>().Where(x => x.AgentId == agentRequestModel.Agent.Id).Select(x => x.SkillId).ToList();
            allyIds.Add(agentRequestModel.Agent.Id);

            var queryable = from intent1 in dc.Table<IntentEntity>()
                            join exp in dc.Table<IntentExpressionEntity>() on intent1.Id equals exp.IntentId
                            where allyIds.Contains(intent1.AgentId)
                            select exp;

            // NLP PIPLINE
            // 预处理语料库，替换实体。
            var expressions = queryable.ToList();

            // 传入句子先分词
            DmAgentRequest agentRequestModel1 = new DmAgentRequest { Agent = agentRequestModel.Agent, ConversationId = agentRequestModel.ConversationId, Text = agentRequestModel.Text };
            var segs = agentRequestModel1.Segment(dc);
            var requestedTextSplitted = segs.Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta.Substring(1)).ToList();
            requestedTextSplitted = requestedTextSplitted.Where(x => !String.IsNullOrEmpty(x.Trim())).ToList();

            Dictionary<String, String> corpus = new Dictionary<string, string>();
            expressions.ForEach(expression =>
            {
                if (!String.IsNullOrEmpty(expression.DataJson))
                {
                    expression.Data = JsonConvert.DeserializeObject<List<DmIntentExpressionItem>>(expression.DataJson);
                    string data = String.Join(" ", expression.Data.SplitToChar().Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta.Substring(1)));
                    corpus.Add(expression.Id, data);
                }
            });

            // 
            TFIDFResolverEngine tfidfResolver = new TFIDFResolverEngine();
            tfidfResolver.Dictionary = corpus;
            List<ResolutionResult> resolutionResults = tfidfResolver.Resolve(String.Join(" ", requestedTextSplitted), false);

            string intentId = expressions.FirstOrDefault(x => x.Id == resolutionResults.First().Key)?.IntentId;

            if (String.IsNullOrEmpty(intentId)) return null;

            var intent = dc.Table<IntentEntity>().First(m => m.Id == intentId);

            var dm = new DomainModel<IntentEntity>(dc, intent.Map<IntentEntity>());
            dm.Load();

            IntentResponseEntity responseModel = dm.Entity.Responses.First();

            // 抽取实体信息并返回缺失的实体
            List<IntentResponseParameterEntity> missingParameters = responseModel.ExtractParameter(dc, segs, agentRequestModel.ConversationId);

            // 填充参数值
            IntentResponseMessageEntity messageModel = responseModel.PostResponse(dc, agentRequestModel1);

            DmAgentResponse response = new DmAgentResponse();
            if (missingParameters.Count > 0)
            {
                // 提示必填信息
                response.Text = missingParameters.Random().Prompts.Random();
            }
            else
            {
                response.Text = messageModel.Speeches.Random();
            }

            // 保存聊天意图状态
            dc.Transaction<IDbRecord4Core>(delegate {
                var conversation = dc.Table<ConversationEntity>().First(x => x.Id == agentRequestModel.ConversationId);
                conversation.IntentId = intent.Id;
            });

            return response;
        }

        /// <summary>
        /// http://www.ruanyifeng.com/blog/2013/03/cosine_similarity.html
        /// http://www.primaryobjects.com/2013/09/13/tf-idf-in-c-net-for-machine-learning-term-frequency-inverse-document-frequency/
        /// </summary>
        /// <param name="corpus"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        private static double CompareSimilarity(List<String> corpus, List<String> requestedTextSplitted, List<String> comparedTextSplitted)
        {
            var items = new List<String>();
            items.AddRange(requestedTextSplitted);
            items.AddRange(comparedTextSplitted);
            var distinctItems = items.Distinct().ToList();

            // term frequency inverted documnet frequency
            double[] vector1 = tfIdfVector(requestedTextSplitted, comparedTextSplitted, distinctItems, corpus);
            double[] vector2 = tfIdfVector(comparedTextSplitted, requestedTextSplitted, distinctItems, corpus);

            double n = 0;
            double n1 = 0;
            double n2 = 0;

            for (int i = 0; i < distinctItems.Count; i++)
            {
                n += vector1[i] * vector2[i];
                n1 += vector1[i] * vector1[i];
                n2 += vector2[i] * vector2[i];
            }

            double cos = n / (Math.Sqrt(n1) * Math.Sqrt(n2));

            return cos;
        }

        private static double[] tfIdfVector(List<String> sentence1, List<String> sentence2, List<String> distinctItems, List<String> corpus)
        {
            Dictionary<string, double> tfidf = new Dictionary<string, double>();

            distinctItems.ForEach(item =>
            {
                int count1 = sentence1.Count(x => x == item);
                int count2 = sentence2.Count(x => x == item);

                double tf = (count1 + 0.0) / (sentence1.Count + sentence2.Count);
                double idf = Math.Log((corpus.Count + 0.0) / (corpus.Count(x => x.Split(' ').Contains(item)) + 1));

                tfidf.Add(item, tf * idf);
            });

            return tfidf.Select(x => x.Value).ToArray();
        }

        public static bool Add(this BundleDomainModel<AgentEntity> dmAgent)
        {
            dmAgent.Entity.Language = "zh-cn";


            if (String.IsNullOrEmpty(dmAgent.Entity.ClientAccessToken))
            {
                dmAgent.Entity.ClientAccessToken = Guid.NewGuid().ToString("N");
            }

            if (String.IsNullOrEmpty(dmAgent.Entity.DeveloperAccessToken))
            {
                dmAgent.Entity.DeveloperAccessToken = Guid.NewGuid().ToString("N");
            }

            return dmAgent.AddEntity();
        }
    }
}
