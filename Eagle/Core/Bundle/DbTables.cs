using Core.Enums;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Bundle
{
    [Table("Bundles")]
    public class BundleEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [MaxLength(50)]
        public String Name { get; set; }
        [Required]
        public string EntityName { get; set; }
        [ForeignKey("BundleId")]
        public List<BundleFieldEntity> Fields { get; set; }
    }

    [Table("BundleFields")]
    public class BundleFieldEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [MaxLength(50)]
        public String Name { get; set; }
        [Required]
        public string BundleId { get; set; }
        [Required]
        public FieldTypes FieldTypeId { get; set; }
    }

    [Table("BundleFieldSettings")]
    public class BundleFieldSettingEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        public string BundleFieldId { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
