using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace EfDbFactory
{
    public class EfDbContext4MySql1 : EfDbContext
    {
        public EfDbContext4MySql1(DbContextOptions options) : base(options) { }
    }

    public class EfDbContext4SqlServer : EfDbContext
    {
        public EfDbContext4SqlServer(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }


    public abstract class EfDbContext : DbContextWithTriggers
    {
        public static String ConnectionString = "";
        public EfDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public List<Type> EntityTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // http://www.learnentityframeworkcore.com/
            // modelBuilder.Entity<TaxonomyTermEntity>().HasIndex(x => x.Name);
            // don't need this code.
            //modelBuilder.Entity<Bundle>().ForSqlServerToTable("Bundles");
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            

            EntityTypes.ForEach(type =>
            {
                var type1 = modelBuilder.Model.FindEntityType(type);
                if(type1 == null)
                {
                    modelBuilder.Model.AddEntityType(type);
                }
                else
                {

                }
                
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
