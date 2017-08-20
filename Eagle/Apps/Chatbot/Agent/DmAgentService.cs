using Apps.Chatbot.Agent;
using Apps.Chatbot.Conversation;
using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Intent;
using Core;
using Core.Interfaces;
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
        public static DmAgentResponse TextRequest(this DmAgentRequest agentRequestModel, CoreDbContext dc, String nerUrl)
        {
            var queryable = from intent1 in dc.Table<IntentEntity>()
                            join exp in dc.Table<IntentExpressionEntity>() on intent1.Id equals exp.IntentId
                            where intent1.AgentId == agentRequestModel.Agent.Id //|| intent.AgentId == Constants.GenesisAgentId
                            select exp;

            // NLP PIPLINE
            // 预处理语料库，替换实体。
            
            var expressions = queryable.ToList();
            List<String> corpus = expressions.Select(x => x.Text).ToList();

            // 加入会话关键词作为输入
            var keywords = dc.Table<ConversationEntity>().Where(x => x.Id == agentRequestModel.ConversationId && !agentRequestModel.Text.Contains(x.Keyword)).OrderBy(x => x.CreatedDate).Select(x => x.Keyword).ToList();
            string keywordsForInput = String.Join(" ", keywords.ToArray());

            // 加入空格，提高NLPIR的实体识别能力
            if (!String.IsNullOrEmpty(keywordsForInput))
            {
                keywordsForInput += " ";
            }

            // 传入句子先分词
            DmAgentRequest agentRequestModel1 = new DmAgentRequest { Agent = agentRequestModel.Agent, ConversationId = agentRequestModel.ConversationId, Text = keywordsForInput + agentRequestModel.Text };
            var requestedTextSplitted = agentRequestModel1.Segment(dc).Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta).ToList();
            requestedTextSplitted = requestedTextSplitted.Where(x => !String.IsNullOrEmpty(x.Trim())).ToList();

            List<IntentExpressionEntity> similarities = new List<IntentExpressionEntity>();

            expressions.ForEach(expression =>
            {
                DmAgentRequest agentRequestModel2 = new DmAgentRequest { Agent = agentRequestModel.Agent, ConversationId = agentRequestModel.ConversationId, Text = expression.Text };
                var comparedTextSplitted = agentRequestModel2.Segment(dc).Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta).ToList();

                IntentExpressionEntity model = expression.Map<IntentExpressionEntity>();
                // 计算出相似度
                model.Similarity = CompareSimilarity(corpus, requestedTextSplitted, comparedTextSplitted);
                if (model.Similarity > 0.5)
                {
                    similarities.Add(model);
                }
            });

            similarities = similarities.OrderByDescending(x => x.Similarity).ToList();

            if (similarities.Count == 0)
            {
                return null;
            }

            expressions = expressions.Where(x => x.Id == similarities.First().Id).ToList();

            if (expressions.Count() == 0) return null;

            var intent = dc.Table<IntentEntity>().First(m => m.Id == expressions.First().IntentId);

            var dm = new DomainModel<IntentEntity>(dc, intent.Map<IntentEntity>());
            dm.Load();

            string keyword = String.Empty;
            // 抽取本次聊天内容的关键词

            if (!String.IsNullOrEmpty(intent.Keyword))
            {
                keyword = intent.Keyword;
            }

            IntentResponseEntity responseModel = dm.Entity.Responses.First();

            // 抽取实体信息并返回缺失的实体
            List<IntentResponseParameterEntity> missingParameters = responseModel.ExtractParameter(dc, agentRequestModel1);

            // 填充参数值
            IntentResponseMessageEntity messageModel = responseModel.PostResponse(dc, agentRequestModel1);

            DmAgentResponse response = new DmAgentResponse();
            if (missingParameters.Count > 0)
            {
                // 提示必填信息
                response.Text = missingParameters.Random().Prompts.Random();
            } else
            {
                response.Text = messageModel.Speeches.Random();
            }

            // 保存聊天记录
            dc.Transaction<IDbRecord4SqlServer>(delegate {
                var conversation = dc.Table<ConversationEntity>().First(x => x.Id == agentRequestModel.ConversationId);
                conversation.Keyword = keyword;
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
                double idf = Math.Log((corpus.Count + 0.0) / (corpus.Count(x => x.Contains(item)) + 1));

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
