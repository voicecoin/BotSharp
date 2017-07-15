using Eagle.DbTables;
using Eagle.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DataContexts
{
    public partial class CoreDbContext : DbContext
    {
        public static IConfigurationRoot Configuration { get; set; }

        public CoreDbContext() { }

        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            string connStr = String.IsNullOrEmpty(connectionString) ? "Server=(localdb)\\MSSQLLocalDB;Database=YayaBot;Trusted_Connection=True;MultipleActiveResultSets=true" : connectionString;

            optionsBuilder.UseSqlServer(connStr);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Agents>().HasIndex(b => b.Name);

            modelBuilder.Entity<Entities>().HasIndex(b => b.Name);
            modelBuilder.Entity<EntityEntries>().HasIndex(b => b.Value);
            modelBuilder.Entity<EntityEntrySynonyms>().HasIndex(b => b.Synonym);

            modelBuilder.Entity<IntentExpressions>().HasIndex(b => b.Text);

            // Json column
            modelBuilder.Entity<Intents>().Property(b => b._Contexts).HasColumnName("Contexts");
            modelBuilder.Entity<Intents>().Property(b => b._Events).HasColumnName("Events");
            modelBuilder.Entity<IntentExpressions>().Property(b => b._Items).HasColumnName("Items");
            modelBuilder.Entity<IntentResponses>().Property(b => b._AffectedContexts).HasColumnName("AffectedContexts");
            modelBuilder.Entity<IntentResponseMessages>().Property(b => b._Speeches).HasColumnName("Speeches");
            modelBuilder.Entity<IntentResponseParameters>().Property(b => b._Prompts).HasColumnName("Prompts");
        }

        public int Transaction(Action action)
        {
            using (IDbContextTransaction transaction = Database.BeginTransaction())
            {
                int affected = 0;
                try
                {
                    action();
                    affected = SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (ex.Message.Contains("See the inner exception for details"))
                    {
                        throw ex.InnerException;
                    }
                    else
                    {
                        throw ex;
                    }
                }

                return affected;
            }
        }

    }
}
