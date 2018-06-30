using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Voicebot.Core.TextToSpeech
{
    [Table("Voiceweb_AgentVoice")]
    public class AgentVoice : DbRecord, IDbRecord
    {
        [Required]
        [StringLength(36)]
        public string AgentId { get; set; }

        [Required]
        [StringLength(32)]
        public string VoiceEngine { get; set; }

        [Required]
        [StringLength(36)]
        public string VoiceId { get; set; }
    }
}
