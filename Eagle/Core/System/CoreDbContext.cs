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
using Microsoft.AspNetCore.Hosting;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Core
{
    public class CoreDbContext : EfDbBase
    {
        public UserEntity CurrentUser { get; set; }
        public static IConfiguration Configuration { get; set; }
        public static IHostingEnvironment Env { get; set; }

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
            else if (db.Equals("MySql"))
            {
                EfDbContext4MySql.ConnectionString = Configuration.GetSection("Database:ConnectionStrings")[db];
                BindDbContext<IDbRecord4MySql, EfDbContext4MySql>(new EfDbBinding
                {
                    connection4Master = new MySqlConnection(EfDbContext4MySql.ConnectionString)
                });
            }

            // init GSMP connection
            /*string gsmp = Configuration.GetSection("Database:SmsOneGsmp").Value;
            BindDbContext<IMySqlGsmpTable, EfDbContext4MySql>(new EfDbBinding
            {
                connection4Master = new MySqlConnection(gsmp),
                connection4Slaves = new List<DbConnection> { new MySqlConnection(gsmp) }
            });*/
        }

        public T GetCache<T>(string key)
        {
            return default(T);
        }

        public bool SetCache(string key, Object value)
        {
            return true;
        }

    }
}
