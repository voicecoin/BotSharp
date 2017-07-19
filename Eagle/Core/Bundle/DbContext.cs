using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DbTables;
using Models;
using Core.DbTables;

namespace Core.DataContexts
{
    public partial class CoreDbContext
    {
        public DbSet<BundleEntity> Bundles { get; set; }
        public DbSet<BundleFieldEntity> BundleFields { get; set; }
        public DbSet<BundleFieldSettingEntity> BundleFieldSettings { get; set; }
    }
}
