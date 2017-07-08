using Eagle.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DomainModels
{
    public class DmIntent
    {
        public DmIntent()
        {
            Contexts = new List<string>();
            Templates = new List<string>();
            UserSays = new List<DmIntentExpression>();
            Responses = new List<DmIntentResponse>();
        }

        public String Id { get; set; }
        public String Name { get; set; }
        public String AgentId { get; set; }
        /// <summary>
        /// Context In
        /// </summary>
        public List<String> Contexts { get; set; }
        public List<String> Templates { get; set; }
        public List<DmIntentExpression> UserSays { get; set; }
        public List<DmIntentResponse> Responses { get; set; }
    }

    public class DmIntentExpression
    {
        public DmIntentExpression()
        {
            Data = new List<DmIntentExpressionItem>();
        }
        public String Id { get; set; }
        public String IntentId { get; set; }
        public Boolean IsTemplate { get; set; }
        public Int32 Count { get; set; }
        public String Text { get; set; }
        public String Template { get; set; }
        public double Similarity { get; set; }
        public List<DmIntentExpressionItem> Data { get; set; }
    }

    public class DmIntentExpressionItem
    {
        public String Id { get; set; }
        public String IntentExpressionId { get; set; }
        public String EntryId { get; set; }
        public String Text { get; set; }
        public String Alias { get; set; }
        public String Meta { get; set; }
        public Boolean UserDefined { get; set; }
        public String EntityId { get; set; }
        /// <summary>
        /// 字符串在句子中的起始位置
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// 实体字符长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 以实体为单位在句子中的位置
        /// </summary>
        public int Unit { get; set; }
    }

    public class DmIntentResponse
    {
        public DmIntentResponse()
        {
            AffectedContexts = new List<DmIntentResponseContext>();
            Messages = new List<DmIntentResponseMessage>();
            Parameters = new List<DmIntentResponseParameter>();
        }
        public String Id { get; set; }
        public String IntentId { get; set; }
        public String Action { get; set; }
        public List<DmIntentResponseContext> AffectedContexts { get; set; }
        public List<DmIntentResponseMessage> Messages { get; set; }
        public List<DmIntentResponseParameter> Parameters { get; set; }
    }

    public class DmIntentResponseContext
    {
        public String Id { get; set; }
        public String IntentResponseId { get; set; }
        public String Name { get; set; }
        public int? Lifespan { get; set; }
    }

    public class DmIntentResponseMessage
    {
        public String Id { get; set; }
        public String IntentResponseId { get; set; }
        public String Speech { get; set; }
        public IntentResponseMessageType Type { get; set; }
        public IntentResponseMessagePlatform Platform { get; set; }
    }

    public class DmIntentResponseParameter
    {
        public DmIntentResponseParameter()
        {
            Prompts = new List<string>();
        }
        public String Id { get; set; }
        public String IntentResponseId { get; set; }
        public String Name { get; set; }
        public Boolean IsList { get; set; }
        public String DataType { get; set; }
        public Boolean Required { get; set; }
        public String Value { get; set; }
        public List<String> Prompts { get; set; }
    }
}
