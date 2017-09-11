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
    public class BundleEntity : DbRecord, IDbRecord4Core
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        [Required]
        public string EntityName { get; set; }
        [ForeignKey("BundleId")]
        public List<BundleFieldEntity> Fields { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<BundleEntity>().Any(x => x.EntityName == EntityName && x.Name == Name);
        }
    }

    [Table("BundleFields")]
    public class BundleFieldEntity : DbRecord, IDbRecord4Core
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        [Required]
        public string BundleId { get; set; }
        [Required]
        public FieldTypes FieldTypeId { get; set; }
        public Boolean Required { get; set; }
        public Boolean Hidden { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<BundleFieldEntity>().Any(x => x.BundleId == BundleId && x.FieldTypeId == FieldTypeId);
        }
    }

    [Table("BundleFieldSettings")]
    public class BundleFieldSettingEntity : DbRecord, IDbRecord4Core
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        [Required]
        public string BundleFieldId { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
