using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Field
{
    public abstract class FieldRepositoryEntity : DbRecord
    {
        [Required]
        public string EntityId { get; set; }
        [Required]
        public string BundleFieldId { get; set; }
    }
}
