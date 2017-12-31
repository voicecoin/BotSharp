using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Registry
{
    /// <summary>
    /// System dictionary
    /// </summary>
    [Table("Registries")]
    public class RegistryEntity : CoreDbRecord, IDbRecord
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        [MaxLength(256, ErrorMessage = "Description cannot be longer than 512 characters.")]
        public String Description { get; set; }
    }

    [Table("RegistryEntries")]
    public class RegistryEntryEntity : CoreDbRecord, IDbRecord
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        [Required]
        public string RegistryId { get; set; }
        public String Value { get; set; }
    }
}
