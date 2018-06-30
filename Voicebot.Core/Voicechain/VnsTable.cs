using BotSharp.Core.Agents;
using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Voicebot.Core.Voicechain
{
    [Table("Voiceweb_VnsTable")]
    public class VnsTable : DbRecord, IDbRecord
    {
        /// <summary>
        /// Entity name
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// Voicechain domain name
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Domain { get; set; }

        [Required]
        [StringLength(36)]
        public String AgentId { get; set; }
    }
}
