using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataFactory
{
    public class EfDbBase
    {
        internal List<EfDbBinding> DbContextBinds;

        public EfDbBase()
        {
            DbContextBinds = new List<EfDbBinding>();
        }

        private List<Type> GetAllEntityTypes(EfDbBinding bind)
        {
            var types1 = Utility.TypeHelper.GetClassesWithInterface(bind.EntityBaseType, "Core");
            var types2 = Utility.TypeHelper.GetClassesWithInterface(bind.EntityBaseType, "Apps");

            types1.AddRange(types2);

            return types1;
        }


        public void BindDbContext(EfDbBinding bind)
        {
            bind.EntityTypeList = GetAllEntityTypes(bind).ToList();

            DbContextBinds.Add(bind);
        }

        public void BindDbContext<TDatabaseType, TDbContextType>(EfDbBinding bind)
        {
            bind.EntityBaseType = typeof(TDatabaseType);
            bind.DbContextType = typeof(TDbContextType);

            bind.EntityTypeList = GetAllEntityTypes(bind).ToList();

            DbContextBinds.Add(bind);
        }

        private EfDbContext GetMaster(Type entityType)
        {
            EfDbBinding binding = DbContextBinds.First(x => (x.EntityBaseType != null && x.EntityBaseType.Equals(entityType)) || (x.EntityTypeList != null && x.EntityTypeList.Contains(entityType)));

            if (binding.DbContextMaster == null)
            {
                DbContextOptions options = new DbContextOptions<EfDbContext>();
                EfDbContext dbContext = Activator.CreateInstance(binding.DbContextType, options) as EfDbContext;
                dbContext.EntityTypes = binding.EntityTypeList;
                binding.DbContextMaster = dbContext;
            }

            return binding.DbContextMaster;
        }

        public bool EnsureCreated(Type type)
        {
            return GetMaster(type).Database.EnsureCreated();
        }

        private EfDbContext GetReader(Type entityType)
        {
            EfDbBinding binding = DbContextBinds.First(x => x.EntityTypeList.Contains(entityType) || x.EntityTypeList.Contains(entityType));

            if (binding.DbContextSlavers == null)
            {
                binding.DbContextSlavers = new List<EfDbContext>();

                DbContextOptions options = new DbContextOptions<EfDbContext>();

                EfDbContext dbContext = Activator.CreateInstance(binding.DbContextType, options) as EfDbContext;
                dbContext.EntityTypes = binding.EntityTypeList;
                binding.DbContextSlavers.Add(dbContext);
            }

            return binding.DbContextSlavers.First();
        }

        /*public IMongoCollection<T> Collection<T>(string collection) where T : class
        {
            EfDbBinding4MongoDb binding = DbContextBinds.First(x => x.GetType().Equals(typeof(EfDbBinding4MongoDb))) as EfDbBinding4MongoDb;
            if (binding.DbContext == null)
            {
                binding.DbContext = new MongoDbContext(binding.ConnectionString);
            }

            return binding.DbContext.Database.GetCollection<T>(collection);
        }

        public ElasticClient EsClient()
        {
            EfDbBinding4ElasticSearch binding = DbContextBinds.First(x => x.GetType().Equals(typeof(EfDbBinding4ElasticSearch))) as EfDbBinding4ElasticSearch;
            if (binding.DbContext == null)
            {
                binding.DbContext = new ElasticSearchDbContext(binding.ConnectionString);
            }

            return binding.DbContext.Client;
        }*/

        public Object Find(Type type, params string[] keys)
        {
            EfDbBinding binding = DbContextBinds.First(x => x.EntityTypeList.Contains(type));
            if (binding.DbContextMaster == null || binding.DbContextMaster.Database.CurrentTransaction == null)
            {
                return GetReader(type).Find(type, keys);
            }
            else
            {
                return GetMaster(type).Find(type, keys);
            }
        }

        public DbSet<T> Table<T>() where T : class
        {
            Type entityType = typeof(T);

            EfDbBinding binding = DbContextBinds.First(x => x.EntityTypeList.Contains(entityType));
            if (binding.DbContextMaster == null || binding.DbContextMaster.Database.CurrentTransaction == null)
            {
                return GetReader(entityType).Set<T>();
            }
            else
            {
                return GetMaster(entityType).Set<T>();
            }
        }

        public int ExecuteSqlCommand<T>(string sql, params object[] parameterms)
        {
            var db = GetMaster(typeof(T)).Database;
            return db.ExecuteSqlCommand(sql, parameterms);
        }

        public int SaveChanges()
        {
            EfDbBinding binding = DbContextBinds.Where(x => x.DbContextType != null).First(x => x.DbContextMaster != null && x.DbContextMaster.Database.CurrentTransaction != null);
            return binding.DbContextMaster.SaveChanges();
        }

        public int Transaction<T>(Action action)
        {
            using (IDbContextTransaction transaction = GetMaster(typeof(T)).Database.BeginTransaction())
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

        public TResult Transaction<T, TResult>(Func<TResult> action)
        {
            using (IDbContextTransaction transaction = GetMaster(typeof(T)).Database.BeginTransaction())
            {
                TResult result = default(TResult);
                try
                {
                    result = action();
                    SaveChanges();
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

                return result;
            }
        }

        public IDbContextTransaction GetDbContextTransaction<T>()
        {
            return GetMaster(typeof(T)).Database.BeginTransaction();
        }
    }
}
