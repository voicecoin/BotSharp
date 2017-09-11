using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Apps.Chatbot.Faq
{
    [Table("Chatbot_Faqs")]
    public class FaqEntity : DbRecord, IDbRecord4Core
    {
        [Required]
        [StringLength(36)]
        public String AgentId { get; set; }

        [Required]
        [MaxLength(256)]
        public String Question { get; set; }

        [Required]
        [MaxLength(256)]
        public String Answer { get; set; }
    }
}
