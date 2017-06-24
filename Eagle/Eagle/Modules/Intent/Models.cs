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
        public List<String> ContextIn { get; set; }
        public List<String> Templates { get; set; }
        public List<IntentExpressionModel> UserSays { get; set; }
        public List<Object> Responses { get; set; }
    }

    public class IntentExpressionModel
    {
        public String Id { get; set; }
        public Boolean IsTemplate { get; set; }
        public Int32 Count { get; set; }
        public List<IntentExpressionItemModel> Data { get; set; }
    }

    public class IntentExpressionItemModel
    {
        public String EntryId { get; set; }
        public String Text { get; set; }
        public String Alias { get; set; }
        public String Meta { get; set; }
        //public Boolean UserDefined { get; set; }
        [JsonIgnore]
        public String EntityId { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
    }
}
