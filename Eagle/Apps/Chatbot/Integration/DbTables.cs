using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Apps.Chatbot.Integration
{
    [Table("Chatbot_AgentPlatforms")]
    public class AgentPlatformEntity : DbRecord, IDbRecord4Core
    {
        [Required]
        [StringLength(36)]
        public String AgentId { get; set; }
        public PlatformType Platform { get; set; }
        public Boolean Enable { get; set; }
        public String Webhook { get; set; }
        public String Token { get; set; }
        public String AppId { get; set; }
        public String EncodingKey { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<AgentPlatformEntity>().Any(x =>　x.AgentId == AgentId && x.Platform == Platform);
        }
    }
}
