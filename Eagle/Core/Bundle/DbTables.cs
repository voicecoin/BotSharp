using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.DbTables
{
    public class BundleEntity : DbRecordWithName
    {
        [Required]
        public string EntityName { get; set; }
        [ForeignKey("BundleId")]
        public List<BundleFieldEntity> Fields { get; set; }
    }

    public class BundleFieldEntity : DbRecordWithName
    {
        [Required]
        public string BundleId { get; set; }
        [Required]
        public FieldTypes FieldTypeId { get; set; }
    }

    public class BundleFieldSettingEntity : DbRecordWithName
    {
        [Required]
        public string BundleFieldId { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
