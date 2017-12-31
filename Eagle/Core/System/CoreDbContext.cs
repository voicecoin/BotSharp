using System;
using Core.Account;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using MySql.Data.MySqlClient;
using EntityFrameworkCore.BootKit;
using Microsoft.Data.Sqlite;

namespace Core
{
    public class CoreDbContext : Database
    {
        public UserEntity CurrentUser { get; set; }
        public static IConfiguration Configuration { get; set; }
        public static IHostingEnvironment Env { get; set; }
        public static String[] Assembles { get; set; }

        public void InitDb()
        {
            string db = Configuration.GetSection("Database:Default").Value;
            string connectionString = Configuration.GetSection("Database:ConnectionStrings")[db];

            if (db.Equals("SqlServer"))
            {
                BindDbContext<IDbRecord, DbContext4SqlServer>(new DatabaseBind
                {
                    MasterConnection = new SqlConnection(connectionString),
                    CreateDbIfNotExist = true,
                    AssemblyNames = Assembles
                });
            }
            else if (db.Equals("Sqlite"))
            {
                connectionString = connectionString.Replace("|DataDirectory|\\", Env.ContentRootPath + "\\App_Data\\");
                BindDbContext<IDbRecord, DbContext4Sqlite>(new DatabaseBind
                {
                    MasterConnection = new SqliteConnection(connectionString),
                    CreateDbIfNotExist = true,
                    AssemblyNames = Assembles
                });
            }
            else if (db.Equals("MySql"))
            {
                BindDbContext<IDbRecord, DbContext4MySql>(new DatabaseBind
                {
                    MasterConnection = new MySqlConnection(connectionString),
                    CreateDbIfNotExist = true,
                    AssemblyNames = Assembles
                });
            }
            else if (db.Equals("InMemory"))
            {
                BindDbContext<IDbRecord, DbContext4Memory>(new DatabaseBind
                {
                    AssemblyNames = Assembles
                });
            }
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
