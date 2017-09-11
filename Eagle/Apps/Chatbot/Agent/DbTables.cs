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
    public class AgentEntity : BundleDbRecord, IDbRecord4Core
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        [MaxLength(256)]
        public String Description { get; set; }
        [MaxLength(8)]
        public String Language { get; set; }
        /// <summary>
        /// 给Yaya平台调用的。
        /// </summary>
        [StringLength(32)]
        public String ClientAccessToken { get; set; }
        /// <summary>
        /// 给SDK调用的
        /// </summary>
        [StringLength(32)]
        public String DeveloperAccessToken { get; set; }

        public bool IsPublic { get; set; }
        [MaxLength(102400)]
        public String Avatar { get; set; }

        public Boolean IsSkillset { get; set; }

        [NotMapped]
        public String Birthday
        {
            get
            {
                return CreatedDate.ToString("yyyy年M月dd日");
            }
        }
        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<AgentEntity>().Any(x => x.Name == Name || x.Id == Id);
        }
    }

    /// <summary>
    /// 机器人结盟，赋予一个机器人具有跟其它机器人一样的能力。
    /// </summary>
    [Table("Chatbot_AgentSkills")]
    public class AgentSkillEntity : DbRecord, IDbRecord4Core
    {
        [Required]
        [StringLength(36)]
        public String AgentId { get; set; }
        [Required]
        [StringLength(36)]
        public String SkillId { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<AgentSkillEntity>().Any(x => x.AgentId == AgentId || x.SkillId == SkillId);
        }
    }
}
