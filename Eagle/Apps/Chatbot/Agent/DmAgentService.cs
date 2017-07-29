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

        public static void Add(this BundleDomainModel<AgentEntity> dmAgent)
        {
            dmAgent.Entity.Avatar = @"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MUUwQ0YzRDE3MjJEMTFFN0JEOTJDNEZBRjIzQjc3NjIiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6MUUwQ0YzRDI3MjJEMTFFN0JEOTJDNEZBRjIzQjc3NjIiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDoxRTBDRjNDRjcyMkQxMUU3QkQ5MkM0RkFGMjNCNzc2MiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDoxRTBDRjNEMDcyMkQxMUU3QkQ5MkM0RkFGMjNCNzc2MiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PgBw7tIAAABgUExURbSyssrKyjAs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";
            dmAgent.AddEntity();
        }
    }
}
