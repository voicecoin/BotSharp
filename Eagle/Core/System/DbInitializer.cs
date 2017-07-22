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

namespace Core
{
    public class DbInitializer
    {
        public static void Initialize(IHostingEnvironment env)
        {
            CoreDbContext dc = new CoreDbContext();
            dc.InitDb();
            dc.CurrentUser = new DmAccount { Id = Constants.SystemUserId };
            dc.EnsureCreated(typeof(UserEntity));

            // Amend table structure
            var dbTableAmends = TypeHelper.GetInstanceWithInterface<IDbTableAmend>("Core");
            dbTableAmends.ToList().ForEach(instance => instance.Amend(dc));

            dbTableAmends = TypeHelper.GetInstanceWithInterface<IDbTableAmend>("Apps");
            dbTableAmends.ToList().ForEach(instance => instance.Amend(dc));

            var instances = TypeHelper.GetInstanceWithInterface<IDbInitializer>("Core");

            // initial app db order by priority
            instances.OrderBy(x => x.Priority).ToList()
                .ForEach(instance =>
                {
                    dc.Transaction<IDbRecord4SqlServer>(() => instance.Load(env, dc));
                });

            instances = TypeHelper.GetInstanceWithInterface<IDbInitializer>("Apps");

            // initial app db order by priority
            instances.OrderBy(x => x.Priority).ToList()
                .ForEach(instance =>
                {
                    dc.Transaction<IDbRecord4SqlServer>(() => instance.Load(env, dc));
                });
        }
    }
}
