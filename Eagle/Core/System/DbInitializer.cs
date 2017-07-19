using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Core.Field;
using Core.Interfaces;
using Core.DbTables;
using Core.DataContexts;

namespace Core
{
    public class DbInitializer
    {
        public static void Initialize(IHostingEnvironment env)
        {
            CoreDbContext dc = new CoreDbContext(new DbContextOptions<CoreDbContext>());
            dc.CurrentUser = new Account.DmAccount { Id = Constants.SystemUserId };
            dc.Database.EnsureCreated();

            var instances = TypeHelper.GetInstanceWithInterface<IDbInitializer>("Core");

            // initial app db order by priority
            instances.OrderBy(x => x.Priority).ToList()
                .ForEach(instance =>
                {
                    dc.Transaction(delegate
                    {
                        instance.Load(env, dc);
                    });
                });

            instances = TypeHelper.GetInstanceWithInterface<IDbInitializer>("Apps");

            // initial app db order by priority
            instances.OrderBy(x => x.Priority).ToList()
                .ForEach(instance =>
                {
                    dc.Transaction(delegate
                    {
                        instance.Load(env, dc);
                    });
                });
        }
    }
}
