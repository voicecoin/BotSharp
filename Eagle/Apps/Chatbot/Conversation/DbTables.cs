using Core;
using Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Chatbot_ConversationParameters.Conversation
{
    [Table("Chatbot_Conversations")]
    public class ConversationEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [StringLength(36)]
        public String AgentId { get; set; }
        /// <summary>
        /// 关键词, 从本次会话Text里提取的关键词，用于作更准备的意图识别。是不是可以用Context来代替？
        /// </summary>
        [MaxLength(32)]
        public String Keyword { get; set; }
    }

    [Table("Chatbot_ConversationMessages")]
    public class ConversationMessageEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [StringLength(36)]
        public String ConversationId { get; set; }
        [Required]
        [StringLength(36)]
        public String Recipient { get; set; }
        [Column("Message")]
        public String MessageJson { get; set; }
        [NotMapped]
        public DmConverstaionContent Message { get; set; }
    }

    [Table("Chatbot_ConversationParameters")]
    public class ConversationParameterEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [StringLength(36)]
        public String ConversationId { get; set; }
        /// <summary>
        /// 可读参数名
        /// </summary>
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }
        /// <summary>
        /// 参数抽取值
        /// </summary>
        [Required]
        [MaxLength(32)]
        public String Value { get; set; }
    }

    public class DmConverstaionContent
    {
        [Required]
        [MaxLength(64)]
        public String Text { get; set; }
        public DmConverstaionContentAttachment Attachement { get; set; }
    }

    public class DmConverstaionContentAttachment
    {
        public ContentAttachmentType Type { get; set; }
        public String Url { get; set; }
    }
}
