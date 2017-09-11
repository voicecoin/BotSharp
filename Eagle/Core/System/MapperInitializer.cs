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
        public void Initialize(IConfigurationRoot config, IHostingEnvironment env)
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
