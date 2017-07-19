using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.DbTables;
using DomainModels;

namespace Core
{
    public static class MapperInitializer
    {
        public static void Initialize()
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
