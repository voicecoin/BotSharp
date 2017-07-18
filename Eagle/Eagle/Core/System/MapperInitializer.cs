using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Eagle.DbTables;
using Eagle.DomainModels;
using Eagle.Utility;

namespace Eagle.Core
{
    public static class MapperInitializer
    {
        public static void Initialize()
        {
            // Initialize AutoMapper
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(Assembly.GetEntryAssembly());
            });
        }
    }
}
