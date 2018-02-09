using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using EntityFrameworkCore.BootKit;
using DotNetToolkit;

namespace Core
{
    public class DbInitializer : IInitializationLoader
    {
        public int Priority => 1;

        public void Initialize(IConfiguration config, IHostingEnvironment env)
        {
            var dc = new DefaultDataContextLoader().GetDefaultDc();

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
