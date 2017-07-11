using Eagle.DbTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DataContexts
{
    public partial class CoreDbContext
    {
        public DbSet<Agents> Agents { get; set; }
    }
}
