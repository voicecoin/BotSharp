using Eagle.DbTables;
using Eagle.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            string connStr = String.IsNullOrEmpty(ConnectionString) ? "Server=(localdb)\\MSSQLLocalDB;Database=YayaBot;Trusted_Connection=True;MultipleActiveResultSets=true" : ConnectionString;

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
