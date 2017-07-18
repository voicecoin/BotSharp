using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DbTables
{
    /// <summary>
    /// System dictionary
    /// </summary>
    public class RegistryEntity : DbRecordWithNameColumn
    {
        [MaxLength(512, ErrorMessage = "Description cannot be longer than 512 characters.")]
        public String Description { get; set; }
    }

    public class RegistryEntryEntity : DbRecordWithNameColumn
    {
        [Required]
        public string RegistryId { get; set; }
        public String Value { get; set; }
        public RegistryEntity Registry { get; set; }
    }
}
