using Eagle.DbTables;
using Eagle.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DbContexts
{
    public partial class DataContexts : DbContext
    {
        public DbSet<Intents> Intents { get; set; }
        public DbSet<IntentInputContexts> IntentInputContexts { get; set; }
        public DbSet<IntentOutputContexts> IntentOutputContexts { get; set; }
        public DbSet<IntentExpressions> IntentExpressions { get; set; }
        public DbSet<IntentExpressionItems> IntentExpressionItems { get; set; }
        public DbSet<IntentEvents> IntentEvents { get; set; }
    }
}
