using Core;
using Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Chatbot.Entity
{
    /// <summary>
    /// 实体，用于做大的分类。
    /// 比如地名，时间，计量单位，机构名，品牌，职位，产品名
    /// </summary>
    [Table("Chatbot_Entities")]
    public class EntityEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [StringLength(36)]
        public String AgentId { get; set; }
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }
        [MaxLength(128)]
        public String Description { get; set; }
        /// <summary>
        /// 枚举，没有同义词
        /// </summary>
        public Boolean IsEnum { get; set; }
        public Boolean IsOverridable { get; set; }
        /// <summary>
        /// Default parameter color
        /// </summary>
        [StringLength(7)]
        public String Color { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<EntityEntity>().Any(x => x.Name == Name);
        }
    }

    /// <summary>
    /// 实体条目。
    /// 比如“北京”，“上海”，这些属于地点。
    /// </summary>
    [Table("Chatbot_EntityEntries")]
    public class EntityEntryEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [StringLength(36)]
        public String EntityId { get; set; }

        [Required]
        [MaxLength(64)]
        public String Value { get; set; }
    }

    /// <summary>
    /// 实体条目标签，用来细分标识条目的特性。
    /// 比如：“奔驰”，可以标识为“汽车”, 属于品牌实体类；“刘德华”，标识为“名人”，属于“人名”实体。
    /// </summary>
    /*public class EntityEntryLables : DbTable
    {
        [Required]
        [JsonIgnore]
        [StringLength(36)]
        public String EntityEntryId { get; set; }
        public String Name { get; set; }
    }*/

    /// <summary>
    /// 实体条目同义词
    /// </summary>
    [Table("Chatbot_EntityEntrySynonyms")]
    public class EntityEntrySynonymEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [StringLength(36)]
        public String EntityEntryId { get; set; }
        [Required]
        [MaxLength(32)]
        public String Synonym { get; set; }
    }
}
