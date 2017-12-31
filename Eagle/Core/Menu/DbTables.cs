using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EntityFrameworkCore.BootKit;

namespace Core.Menu
{
    public class MenuEntity : DbRecord, IDbRecord
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        [MaxLength(64)]
        public string Description { get; set; }
        [MaxLength(16)]
        public string Icon { get; set; }
        [MaxLength(64)]
        public String Link { get; set; }
    }
}
