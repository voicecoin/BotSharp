using Eagle.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Models
{
    public class IntentModel
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String AgentId { get; set; }
        /// <summary>
        /// Context In
        /// </summary>
        public List<String> Contexts { get; set; }
        public List<String> Templates { get; set; }
        public List<IntentExpressionModel> UserSays { get; set; }
        public List<IntentResponseModel> Responses { get; set; }
    }

    public class IntentExpressionModel
    {
        public String Id { get; set; }
        public String IntentId { get; set; }
        public Boolean IsTemplate { get; set; }
        public Int32 Count { get; set; }
        public List<IntentExpressionItemModel> Data { get; set; }
    }

    public class IntentExpressionItemModel
    {
        public String Id { get; set; }
        public String IntentExpressionId { get; set; }
        public String EntryId { get; set; }
        public String Text { get; set; }
        public String Alias { get; set; }
        public String Meta { get; set; }
        //public Boolean UserDefined { get; set; }
        public String EntityId { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
    }

    public class IntentResponseModel
    {
        public String Id { get; set; }
        public String IntentId { get; set; }
        public String Action { get; set; }
        public List<IntentResponseContextModel> AffectedContexts { get; set; }
        public List<IntentResponseMessageModel> Messages { get; set; }
        public List<IntentResponseParameterModel> Parameters { get; set; }
    }

    public class IntentResponseContextModel
    {
        public String Id { get; set; }
        public String IntentResponseId { get; set; }
        public String Name { get; set; }
        public int? Lifespan { get; set; }
    }

    public class IntentResponseMessageModel
    {
        public String Id { get; set; }
        public String IntentResponseId { get; set; }
        public String Speech { get; set; }
        public IntentResponseMessageType Type { get; set; }
        public IntentResponseMessagePlatform Platform { get; set; }
    }

    public class IntentResponseParameterModel
    {
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
