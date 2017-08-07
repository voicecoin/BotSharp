using Apps.Chatbot.Agent;
using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Intent;
using Core;
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
            var queryable = from intent in dc.Table<IntentEntity>()
                            join exp in dc.Table<IntentExpressionEntity>() on intent.Id equals exp.IntentId
                            where intent.AgentId == agentRequestModel.Agent.Id //|| intent.AgentId == Constants.GenesisAgentId
                            select exp;

            // 精确匹配
            var intents = queryable.Where(x => x.Text == agentRequestModel.Text).ToList();

            // 相似匹配
            if (intents.Count() == 0)
            {
                // 预处理语料库，替换实体。
                List<String> corpus = new List<String>();
                intents = queryable.ToList();
                intents.ForEach(exp => {
                    if (exp.Data == null || exp.Data.Count() == 0)
                    {
                        corpus.Add(exp.Text);
                    }
                    else
                    {
                        corpus.Add(String.Join("", exp.Data.Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta)));
                    }
                });

                // 传入句子先分词
                DmAgentRequest agentRequestModel1 = new DmAgentRequest { Text = agentRequestModel.Text };
                var requestedTextSplitted = agentRequestModel1.Segment(dc).Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta).ToList();

                List<IntentExpressionEntity> similarities = new List<IntentExpressionEntity>();

                intents.ForEach(expression =>
                {
                    DmAgentRequest agentRequestModel2 = new DmAgentRequest { Text = expression.Text };
                    var comparedTextSplitted = agentRequestModel2.Segment(dc).Select(x => String.IsNullOrEmpty(x.Meta) ? x.Text : x.Meta).ToList();

                    IntentExpressionEntity model = expression.Map<IntentExpressionEntity>();
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

                intents = queryable.Where(x => x.Id == similarities.First().Id).ToList();
            }


            if (intents.Count() == 0) return null;

            var intentRecord = dc.Table<IntentEntity>().First(m => m.Id == intents.First().IntentId);

            var dm = new DomainModel<IntentEntity>(dc, intentRecord.Map<IntentEntity>());
            dm.Load();

            IntentResponseEntity responseModel = dm.Entity.Responses.First();

            try
            {
                responseModel.ExtractParameter(dc, agentRequestModel);
            }
            catch (MissingParameterException ex)
            {
                return new DmAgentResponse { Text = ex.Message };
            }
            

            IntentResponseMessageEntity messageModel = responseModel.PostResponse(dc, agentRequestModel);

            return new DmAgentResponse { Text = messageModel.Speeches.Random() };
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
            dmAgent.Entity.ClientAccessToken = Guid.NewGuid().ToString("N");
            dmAgent.Entity.DeveloperAccessToken = Guid.NewGuid().ToString("N");

            return dmAgent.AddEntity();
        }
    }
}
