using Apps.Chatbot.Intent;
using Core;
using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Apps.Chatbot.Faq
{
    [Table("Chatbot_Faqs")]
    public class FaqEntity : DbRecord, IDbRecord
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

        [JsonIgnore]
        [Column("Data")]
        [MaxLength]
        public String DataJson { get; set; }
        [NotMapped]
        public List<DmIntentExpressionItem> Data { get; set; }

        public bool IsExist(Database dc)
        {
            return dc.Table<FaqEntity>().Any(x => x.AgentId == AgentId && x.Question == Question);
        }
    }
}
