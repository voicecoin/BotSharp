using Core.Interfaces;
using EntityFrameworkCore.BootKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Bundle
{
    public interface IBundlable
    {
        string BundleId { get; set; }
        void LoadFieldRecords(CoreDbContext dc);
    }

    public interface IBundlable<T> where T : IDbRecord
    {
        string BundleId { get; set; }
        void LoadFieldRecords();
    }
}
