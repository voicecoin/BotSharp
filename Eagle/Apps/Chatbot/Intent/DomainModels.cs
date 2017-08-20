using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Chatbot.Intent
{
    public class DmIntentExpressionItem
    {
        /// <summary>
        /// Original Text
        /// </summary>
        public String Text { get; set; }
        /// <summary>
        /// Converted entity entry value if recognized.
        /// 只有在同义词的时候有用。 比如把"番茄"都转换成"西红柿"
        /// </summary>
        public String Value { get; set; }
        /// <summary>
        /// 参数名字，把识别出的实体放入参数。
        /// </summary>
        public String Alias { get; set; }
        /// <summary>
        /// Entity Name
        /// </summary>
        public String Meta { get; set; }
        public Boolean UserDefined { get; set; }
        /// <summary>
        /// Entity Color
        /// </summary>
        public String Color { get; set; }
        /// <summary>
        /// 字符串在句子中的起始位置
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// 实体字符长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 是否包含同义词
        /// </summary>
        public Boolean IsEnum { get; set; }
    }

    public class DmIntentResponseContext
    {
        public String Name { get; set; }
        public int? Lifespan { get; set; }
    }
}
