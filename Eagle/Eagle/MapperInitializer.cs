using AutoMapper;
using Eagle.DbTables;
using Eagle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle
{
    public class MapperInitializer
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Agents, AgentModel>();
                cfg.CreateMap<IntentExpressionItemModel, IntentExpressionItems>();
                cfg.CreateMap<Entities, EntityModel>();
            });
        }
    }
}
