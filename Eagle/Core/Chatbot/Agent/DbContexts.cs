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
        public DbSet<Agents> Chatbot_Agents { get; set; }
    }
}
