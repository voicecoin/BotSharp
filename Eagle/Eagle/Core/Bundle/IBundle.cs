using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Eagle.DataContexts;

namespace Eagle.Core
{
    public interface IBundle
    {
        [Required]
        string BundleId { get; set; }
        [NotMapped]
        List<Object> FieldRecords { get; set; }
        void LoadFieldRecords(CoreDbContext dc);
    }
}