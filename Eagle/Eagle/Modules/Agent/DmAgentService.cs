using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.Enums;
using Eagle.DomainModels;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DmServices
{
    public static class DmAgentService
    {
        public static DmAgentResponse TextRequest(this DmAgentRequest agentRequestModel, DataContexts dc)
        {
            var queryable = from intent in dc.Intents
                            join exp in dc.IntentExpressions on intent.Id equals exp.IntentId
                            where intent.AgentId == agentRequestModel.Agent.Id || intent.AgentId == Constants.GenesisAgentId
                            select exp;

            // 精确匹配
            var intents = queryable.Where(x => x.Text == agentRequestModel.Text || x.Template == agentRequestModel.Text).ToList();

            // 相似匹配
            if (intents.Count() == 0)
            {
                List<DmIntentExpression> similarities = new List<DmIntentExpression>();
                intents = queryable.ToList();
                intents.ForEach(expression =>
                {
                    DmIntentExpression model = expression.Map<DmIntentExpression>();
                    model.Similarity = CompareSimilarity(intents.Select(x => x.Text).ToList(), agentRequestModel.Text, expression.Text, dc);
                    if(model.Similarity > 0.6)
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


            if (intents.Count == 0) return null;

            var intentRecord = dc.Intents.First(m => m.Id == intents.First().IntentId);

            var intentModel = intentRecord.Map<DmIntent>();
            intentModel.Load(dc);

            DmIntentResponse responseModel = intentModel.Responses.First();
            DmIntentResponseMessage messageModel = responseModel.PostResponse(dc, agentRequestModel.Agent);

            return new DmAgentResponse { Text = messageModel.Speech };
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
        private static double CompareSimilarity(List<String> corpus, String source, String destination, DataContexts dc)
        {
            DmAgentRequest agentRequestModel1 = new DmAgentRequest { Text = source };
            var items1 = agentRequestModel1.Segment(dc).Select(x => x.Text).ToList();

            DmAgentRequest agentRequestModel2 = new DmAgentRequest { Text = destination };
            var items2 = agentRequestModel2.Segment(dc).Select(x => x.Text).ToList();

            var items = new List<String>();
            items.AddRange(items1);
            items.AddRange(items2);
            var distinctItems = items.Distinct().ToList();

            // term frequency inverted documnet frequency
            double[] vector1 = tfIdfVector(items1, items2, distinctItems, corpus);
            double[] vector2 = tfIdfVector(items2, items1, distinctItems, corpus);

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
    }
}
