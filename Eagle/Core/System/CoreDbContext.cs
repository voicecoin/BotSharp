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
using Microsoft.AspNetCore.Hosting;

namespace Core
{
    public class CoreDbContext : EfDbBase
    {
        public UserEntity CurrentUser { get; set; }
        public static IConfigurationRoot Configuration { get; set; }
        public static IHostingEnvironment Env { get; set; }
        public IMemcachedClient MemcachedClient { get; set; }

        public void InitDb()
        {
            string db = Configuration.GetSection("Database:Default").Value;
            if (db.Equals("SqlServer"))
            {
                EfDbContext4SqlServer.ConnectionString = Configuration.GetSection("Database:ConnectionStrings")[db];
                BindDbContext<IDbRecord4Core, EfDbContext4SqlServer>(new EfDbBinding
                {
                    connection4Master = new SqlConnection(EfDbContext4SqlServer.ConnectionString)
                });
            }
            else if (db.Equals("Sqlite"))
            {
                EfDbContext4Sqlite.ConnectionString = Configuration.GetSection("Database:ConnectionStrings")[db];
                EfDbContext4Sqlite.ConnectionString = EfDbContext4Sqlite.ConnectionString.Replace("|DataDirectory|\\", Env.ContentRootPath + "\\App_Data\\");
                BindDbContext<IDbRecord4Core, EfDbContext4Sqlite>(new EfDbBinding
                {
                    connection4Master = new SqlConnection(EfDbContext4SqlServer.ConnectionString)
                });
            }
            /*else if (db.Equals("MySql"))
            {
                EfDbContext4MySql.ConnectionString = Configuration.GetSection("Database:ConnectionStrings")[db];
                BindDbContext<IDbRecord4MySql, EfDbContext4MySql>(new EfDbBinding
                {
                    connection4Master = new MySqlConnection(EfDbContext4MySql.ConnectionString)
                });
            }*/
        }

        public T GetCache<T>(string key)
        {
            key = key.Replace(" ", String.Empty);
            return MemcachedClient == null ? default(T) : MemcachedClient.Get<T>(key);
        }

        public bool SetCache(string key, Object value)
        {
            key = key.Replace(" ", String.Empty);
            return MemcachedClient == null ? false : MemcachedClient.Store(StoreMode.Set, key, value);
        }

    }
}
