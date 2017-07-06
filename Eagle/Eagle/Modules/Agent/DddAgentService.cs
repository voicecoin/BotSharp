using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.Enums;
using Eagle.Models;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DddServices
{
    public static class DddAgentService
    {
        public static AgentResponseModel TextRequest(this AgentRequestModel agentRequestModel, DataContexts dc)
        {
            var queryable = from intent in dc.Intents
                            join exp in dc.IntentExpressions on intent.Id equals exp.IntentId
                            where intent.AgentId == agentRequestModel.Agent.Id || intent.AgentId == Constants.GenesisAgentId
                            select exp;

            // 精确匹配
            var intents = queryable.Where(x => x.Text == agentRequestModel.Text).ToList();

            List<IntentExpressionItemModel> items = null;

            if (intents.Count() == 0)
            {
                items = agentRequestModel.PosTagger(dc);
                String template = items.GetTemplateString();
                intents = queryable.Where(x => x.Template == agentRequestModel.Text).ToList();
            }

            // 相似匹配
            if (intents.Count() == 0)
            {
                List<IntentExpressionModel> similarities = new List<IntentExpressionModel>();
                intents = queryable.ToList();
                intents.ForEach(expression =>
                {
                    IntentExpressionModel model = expression.Map<IntentExpressionModel>();
                    model.Similarity = CompareSimilarity(intents.Select(x => x.Template).ToList(), agentRequestModel.Text, expression.Template, dc);

                    similarities.Add(model);
                });

                similarities = similarities.OrderByDescending(x => x.Similarity).ToList();

                intents = queryable.Where(x => x.Id == similarities.First().Id).ToList();
            }


            if (intents.Count == 0) return null;

            var intentRecord = dc.Intents.First(m => m.Id == intents.First().IntentId);

            var intentModel = intentRecord.Map<IntentModel>();
            intentModel.Load(dc);

            IntentResponseModel responseModel = intentModel.Responses.First();
            IntentResponseMessageModel messageModel = responseModel.PostResponse(dc, agentRequestModel.Agent);

            return new AgentResponseModel { Text = messageModel.Speech };
        }

        /// <summary>
        /// http://www.ruanyifeng.com/blog/2013/03/cosine_similarity.html
        /// http://www.primaryobjects.com/2013/09/13/tf-idf-in-c-net-for-machine-learning-term-frequency-inverse-document-frequency/
        /// </summary>
        /// <param name="corpus"></param>
        /// <param name="sentence1"></param>
        /// <param name="sentence2"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        private static double CompareSimilarity(List<String> corpus, String sentence1, String sentence2, DataContexts dc)
        {
            AgentRequestModel agentRequestModel1 = new AgentRequestModel { Text = sentence1 };
            var items1 = agentRequestModel1.PosTagger(dc).Select(x => x.Text).ToList();

            AgentRequestModel agentRequestModel2 = new AgentRequestModel { Text = sentence2 };
            var items2 = agentRequestModel2.PosTagger(dc).Select(x => x.Text).ToList();

            var items = new List<String>();
            items.AddRange(items1);
            items.AddRange(items2);
            var distinctItems = items.Distinct().ToList();

            // term frequency
            Dictionary<string, int> tfidf1 = new Dictionary<string, int>();
            distinctItems.ForEach(item =>
            {
                int count1 = items1.Count(x => x == item);
                /*int count2 = items2.Count(x => x == item);

                double tf = count1 / (items1.Count + items2.Count);
                double idf = Math.Log(corpus.Count / (count2 + 1));*/

                tfidf1.Add(item, count1);
            });

            Dictionary<string, int> tfidf2 = new Dictionary<string, int>();
            distinctItems.ForEach(item =>
            {
                int count = items2.Count(x => x == item);
                tfidf2.Add(item, count);
            });



            int[] vector1 = tfidf1.Select(x => x.Value).ToArray();
            int[] vector2 = tfidf2.Select(x => x.Value).ToArray();

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
    }
}
