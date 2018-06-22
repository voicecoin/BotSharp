using BotSharp.Core.Agents;
using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
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
        public string Name { get; set; }

        /// <summary>
        /// Voicechain domain name
        /// </summary>
        public string Domain { get; set; }

        public String AgentId { get; set; }
    }
}
