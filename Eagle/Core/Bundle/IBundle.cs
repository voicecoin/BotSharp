using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.DataContexts;

namespace Core.Bundle
{
    public interface IBundlable
    {
        string BundleId { get; set; }
        void LoadFieldRecords(CoreDbContext dc);
    }
}
