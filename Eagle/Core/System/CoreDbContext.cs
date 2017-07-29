using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Account;
using Microsoft.Extensions.Configuration;
using DataFactory;
using System.Data.SqlClient;
using Core.Interfaces;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace Core
{
    public class CoreDbContext : EfDbBase
    {
        public DmAccount CurrentUser { get; set; }
        public static IConfigurationRoot Configuration { get; set; }
        public IMemcachedClient MemcachedClient { get; set; }

        public void InitDb()
        {
            EfDbContext4SqlServer.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            BindDbContext<IDbRecord4SqlServer, EfDbContext4SqlServer>(new EfDbBinding
            {
                connection4Master = new SqlConnection(EfDbContext4SqlServer.ConnectionString)
            });
        }

        public T GetCache<T>(string key)
        {
            return MemcachedClient.Get<T>(key);
        }

        public bool SetCache(string key, Object value)
        {
            return MemcachedClient.Store(StoreMode.Set, key, value);
        }
    }

    /*public partial class CoreDbContext : DbContextWithTriggers
    {
        public DmAccount CurrentUser { get; set; }
        public static IConfigurationRoot Configuration { get; set; }
        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // http://www.learnentityframeworkcore.com/
            // modelBuilder.Entity<TaxonomyTermEntity>().HasIndex(x => x.Name);
            // don't need this code.
            //modelBuilder.Entity<Bundle>().ForSqlServerToTable("Bundles");
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
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
    }*/
}
