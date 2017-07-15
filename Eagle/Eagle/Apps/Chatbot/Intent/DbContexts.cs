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
        public DbSet<Intents> Intents { get; set; }
        public DbSet<IntentExpressions> IntentExpressions { get; set; }
        public DbSet<IntentResponses> IntentResponses { get; set; }
        public DbSet<IntentResponseMessages> IntentResponseMessages { get; set; }
        public DbSet<IntentResponseParameters> IntentResponseParameters { get; set; }
    }
}
