using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DomainModels;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Core
{
    public class MapperInitializer : IInitializationLoader
    {
        public int Priority => 10;

        public void Initialize(IHostingEnvironment env)
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
