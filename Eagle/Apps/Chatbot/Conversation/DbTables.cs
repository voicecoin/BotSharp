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

namespace Apps.Chatbot.Conversation
{
    [Table("Chatbot_Conversations")]
    public class ConversationEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [StringLength(36)]
        public String SessionId { get; set; }
        [Required]
        [StringLength(36)]
        public String AgentId { get; set; }
    }

    [Table("Chatbot_ConversationMessages")]
    public class ConversationMessageEntity : DbRecord, IDbRecord4SqlServer
    {
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
        public String ResponseParameter { get; set; }
        public String Value { get; set; }
    }

    public class DmConverstaionContent
    {
        public String Text { get; set; }
        public DmConverstaionContentAttachment Attachement { get; set; }
    }

    public class DmConverstaionContentAttachment
    {
        public ContentAttachmentType Type { get; set; }
        public String Url { get; set; }
    }
}
