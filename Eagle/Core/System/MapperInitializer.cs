using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Core
{
    public class MapperInitializer : IInitializationLoader
    {
        public int Priority => 100;

        public void Initialize(IConfiguration config, IHostingEnvironment env)
        {
            // Initialize AutoMapper
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(Assembly.Load(new AssemblyName("Core")));
                cfg.AddProfiles(Assembly.Load(new AssemblyName("Apps")));
            });
        }
    }
}
