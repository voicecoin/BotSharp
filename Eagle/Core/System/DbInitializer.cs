using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Core.Field;
using Core.Interfaces;
using Core.Account;
using Microsoft.Extensions.Configuration;
using EntityFrameworkCore.BootKit;

namespace Core
{
    public class DbInitializer : IInitializationLoader
    {
        public int Priority => 1;

        public void Initialize(IConfiguration config, IHostingEnvironment env)
        {
            CoreDbContext dc = new CoreDbContext();
            dc.InitDb();
            dc.CurrentUser = new UserEntity { Id = Constants.SystemUserId };

            // Amend table structure
            var dbTableAmends = TypeHelper.GetInstanceWithInterface<IDbTableAmend>("Core");
            dbTableAmends.ToList().ForEach(instance => instance.Amend(dc));

            dbTableAmends = TypeHelper.GetInstanceWithInterface<IDbTableAmend>("Apps");
            dbTableAmends.ToList().ForEach(instance => instance.Amend(dc));

            var instances = TypeHelper.GetInstanceWithInterface<IHookDbInitializer>("Core");

            // initial app db order by priority
            instances.OrderBy(x => x.Priority).ToList()
                .ForEach(instance => dc.Transaction<IDbRecord>(() => instance.Load(env, config, dc)));

            instances = TypeHelper.GetInstanceWithInterface<IHookDbInitializer>("Apps");

            // initial app db order by priority
            instances.OrderBy(x => x.Priority).ToList()
                .ForEach(instance =>
                {
                    dc.Transaction<IDbRecord>(() => instance.Load(env, config, dc));
                });
        }
    }
}
