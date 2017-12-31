using Apps.Chatbot.Agent;
using Apps.Chatbot.Conversation;
using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Faq;
using Apps.Chatbot.Intent;
using Core;
using Core.Interfaces;
using EntityFrameworkCore.BootKit;
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
            DmAgentResponse response = new DmAgentResponse();

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

            // 加入快速问答语料
            var faqs = dc.Table<FaqEntity>().Where(x => allyIds.Contains(x.AgentId))
                .Select(x => new IntentExpressionEntity
                {
                    Id = x.Id,
                    IsFaq = true,
                    Text = x.Question,
                    FaqAnswer = x.Answer,
                    DataJson = x.DataJson
                }).ToList();
            expressions.AddRange(faqs);

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

            // 计算相似度
            string document = String.Join(" ", requestedTextSplitted);

            BayesResolverEngine bayesResolver = new BayesResolverEngine();
            bayesResolver.Dictionary = corpus;
            var resolutionResults = bayesResolver.Resolve(document, false);

            // 处理少语料无法匹配的情况
            if(resolutionResults.Count == 0)
            {
                LevenshteinResolverEngine levenshteinResolver = new LevenshteinResolverEngine();
                levenshteinResolver.Dictionary = corpus;
                resolutionResults = levenshteinResolver.Resolve(document, false);
            }

            string expressionId = resolutionResults.First().Key;

            // 优先匹配快速问答并返回
            var faq = faqs.FirstOrDefault(x => x.Id == expressionId);
            if(faq != null)
            {
                response.Text = faq.FaqAnswer;
                return response;
            }

            string intentId = expressions.FirstOrDefault(x => x.Id == expressionId)?.IntentId;

            if (String.IsNullOrEmpty(intentId)) return null;

            var intent = dc.Table<IntentEntity>().First(m => m.Id == intentId);

            var dm = new DomainModel<IntentEntity>(dc, intent.Map<IntentEntity>());
            dm.Load();

            IntentResponseEntity responseModel = dm.Entity.Responses.First();

            // 上文Context
            var context = dc.Table<ConversationEntity>().First(x => x.Id == agentRequestModel.ConversationId);
            if (!String.IsNullOrEmpty(context.ContextsJson))
            {
                context.Contexts = JsonConvert.DeserializeObject<List<DmIntentResponseContext>>(context.ContextsJson);
            }
            else
            {
                context.Contexts = new List<DmIntentResponseContext>();
            }

            // 抽取实体信息并返回缺失的实体
            List<IntentResponseParameterEntity> parameters = responseModel.ExtractParameter(dc, context.Contexts, segs, agentRequestModel.ConversationId);

            // 填充参数值
            IntentResponseMessageEntity messageModel = responseModel.PostResponse(dc, agentRequestModel1);

            
            if (parameters.Count(x => String.IsNullOrEmpty(x.Value))> 0)
            {
                // 提示必填信息
                response.Text = parameters.Where(x => String.IsNullOrEmpty(x.Value)).Random().Prompts.Random();
            }
            else
            {
                response.Text = messageModel.Speeches.Random();
            }

            // 保存意图上下文
            dc.Transaction<IDbRecord>(delegate {
                var conversation = dc.Table<ConversationEntity>().First(x => x.Id == agentRequestModel.ConversationId);
                conversation.IntentId = intent.Id;
                if (!String.IsNullOrEmpty(conversation.ContextsJson))
                {
                    conversation.Contexts = JsonConvert.DeserializeObject<List<DmIntentResponseContext>>(conversation.ContextsJson);
                }
                else
                {
                    conversation.Contexts = new List<DmIntentResponseContext>();
                }

                if(responseModel.AffectedContexts.Count > 0)
                {
                    conversation.Contexts.AddRange(responseModel.AffectedContexts);
                }

                conversation.ContextsJson = JsonConvert.SerializeObject(conversation.Contexts);
            });

            // 对人物机器人的回复做转换
            if(intent.AgentId == "b8d4d157-611a-40cb-ad5a-142987a73b8a")
            {
                PeopleResponseParser peopleResponseParser = new PeopleResponseParser();
                response.Text = peopleResponseParser.FillReplyTemplate(dc, parameters.Where(x => !String.IsNullOrEmpty(x.Value)).ToList(), response.Text);
            }

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
