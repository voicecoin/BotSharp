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

namespace Apps.Chatbot.Agent
{
    [Table("Chatbot_Agents")]
    public class AgentEntity : BundleDbRecord, IDbRecord4SqlServer
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        [MaxLength(256)]
        public String Description { get; set; }
        [MaxLength(8)]
        public String Language { get; set; }

        [Required]
        [StringLength(36)]
        public String UserId { get; set; }
        /// <summary>
        /// 给Yaya平台调用的。
        /// </summary>
        [Required]
        [StringLength(32)]
        public String ClientAccessToken { get; set; }
        /// <summary>
        /// 给SDK调用的
        /// </summary>
        [Required]
        [StringLength(32)]
        public String DeveloperAccessToken { get; set; }

        public bool IsPublic { get; set; }
        [MaxLength(4096)]
        public String Avatar { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<AgentEntity>().Any(x => x.Name == Name);
        }
    }
}
