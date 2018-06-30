using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Voicebot.Core.Chatbots.SkillSets
{
    [Table("Voiceweb_SkillSetOfAgent")]
    public class SkillSetOfAgent : DbRecord, IDbRecord
    {
        [Required]
        [StringLength(36)]
        public string SkillSetId { get; set; }

        [Required]
        [StringLength(36)]
        public string AgentId { get; set; }
    }
}
