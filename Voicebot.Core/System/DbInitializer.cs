using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EntityFrameworkCore.BootKit;
using DotNetToolkit;
using Voicebot.Core.Interfaces;

namespace Voicebot.Core
{
    public class DbInitializer : IInitializationLoader
    {
        public int Priority => 1;

        public void Initialize(IConfiguration config, IHostingEnvironment env)
        {
            var dc = new DefaultDataContextLoader().GetDefaultDc();

            // Amend table structure
            var instances = TypeHelper.GetInstanceWithInterface<IHookDbInitializer>("Voicebot.Core");

            // initial app db order by priority
            instances.OrderBy(x => x.Priority).ToList()
                .ForEach(instance => dc.Transaction<IDbRecord>(() => instance.Load(dc)));
        }
    }
}
