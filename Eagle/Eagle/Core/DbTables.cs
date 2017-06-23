using Eagle.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DbTables
{
    public abstract class DbTable
    {
        public DbTable()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.UtcNow;
        }
        
        [Key]
        [StringLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String Id { get; set; }

        [Required]
        [StringLength(36)]
        public String CreatedUserId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public bool Disabled { get; set; }
    }
}
