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
        public static string ConnectionString { get; set; }

        public DataContexts() { }

        public DataContexts(DbContextOptions<DataContexts> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = String.IsNullOrEmpty(ConnectionString) ? "Server=(localdb)\\MSSQLLocalDB;Database=Eagle;Trusted_Connection=True;MultipleActiveResultSets=true" : ConnectionString;

            optionsBuilder.UseSqlServer(connStr);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
