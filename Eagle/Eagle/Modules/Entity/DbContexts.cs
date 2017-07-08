using Eagle.DbTables;
using Eagle.Modules.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DbContexts
{
    public partial class DataContexts : DbContext
    {
        public DbSet<Entities> Entities { get; set; }
        public DbSet<EntityEntries> EntityEntries { get; set; }
        //public DbSet<EntityEntryLables> EntityEntryLables { get; set; }
        public DbSet<EntityEntrySynonyms> EntityEntrySynonyms { get; set; }
    }
}
