using Core.DbTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.DataContexts
{
    public partial class CoreDbContext
    {
        public DbSet<Intents> Chatbot_Intents { get; set; }
        public DbSet<IntentExpressions> Chatbot_IntentExpressions { get; set; }
        public DbSet<IntentResponses> Chatbot_IntentResponses { get; set; }
        public DbSet<IntentResponseMessages> Chatbot_IntentResponseMessages { get; set; }
        public DbSet<IntentResponseParameters> Chatbot_IntentResponseParameters { get; set; }
    }
}
