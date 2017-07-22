using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Page
{
    public class PageEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        public String Description { get; set; }
    }
}
