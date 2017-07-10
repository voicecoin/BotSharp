using Eagle.DbTables;
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
        public DbSet<IntentExpressions> IntentExpressions { get; set; }
        public DbSet<IntentExpressionItems> IntentExpressionItems { get; set; }
        public DbSet<IntentEvents> IntentEvents { get; set; }
        public DbSet<IntentResponses> IntentResponses { get; set; }
        public DbSet<IntentResponseContexts> IntentResponseContexts { get; set; }
        public DbSet<IntentResponseMessages> IntentResponseMessages { get; set; }
        public DbSet<IntentResponseMessageContents> IntentResponseMessageContents { get; set; }
        public DbSet<IntentResponseParameters> IntentResponseParameters { get; set; }
        public DbSet<IntentResponseParameterPrompts> IntentResponseParameterPrompts { get; set; }
    }
}
