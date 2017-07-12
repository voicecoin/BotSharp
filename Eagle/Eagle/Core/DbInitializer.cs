using Eagle.Apps.Chatbot.DmServices;
using Eagle.Apps.Chatbot.DomainModels;
using Eagle.DataContexts;
using Eagle.DbTables;
using Eagle.DmServices;
using Eagle.DomainModels;
using Eagle.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Eagle.Core
{
    public class DbInitializer
    {
        public static void Initialize(IHostingEnvironment env)
        {
            CoreDbContext dc = new CoreDbContext(new DbContextOptions<CoreDbContext>());
            dc.Database.EnsureCreated();

            var instances = TypeHelper.GetInstanceWithInterface<IDbInitializer>();

            instances.ForEach(instance => {
                dc.Transaction(delegate {
                    instance.Load(env, dc);
                });
            });
        }
    }

    public interface IDbInitializer
    {
        void Load(IHostingEnvironment env, CoreDbContext dc);
    }
}
