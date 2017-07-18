using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DbTables
{
    public abstract class FieldRepositoryEntity : DbRecord
    {
        [Required]
        public string EntityId { get; set; }
        [Required]
        public string BundleFieldId { get; set; }
    }
}
