using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DbTables
{
    public class Agents : DbTable
    {
        [Required]
        [MaxLength(32)]
        public String Name { get; set; }
        [MaxLength(256)]
        public String Description { get; set; }

        [Required]
        [StringLength(32)]
        public String ClientAccessToken { get; set; }
        [Required]
        [StringLength(32)]
        public String DeveloperAccessToken { get; set; }
    }
}
