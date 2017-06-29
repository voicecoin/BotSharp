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
                cfg.CreateMap<Agents, AgentModel>().ReverseMap();
                cfg.CreateMap<Agents, AgentDetailModel>().ReverseMap();
                cfg.CreateMap<Intents, IntentModel>().ReverseMap();
                cfg.CreateMap<IntentExpressions, IntentExpressionModel>().ReverseMap();
                cfg.CreateMap<IntentExpressionItems, IntentExpressionItemModel>().ReverseMap();
                cfg.CreateMap<IntentResponses, IntentResponseModel>().ReverseMap();
                cfg.CreateMap<IntentResponseContexts, IntentResponseContextModel>().ReverseMap();
                cfg.CreateMap<IntentResponseMessages, IntentResponseMessageModel>().ReverseMap();
                cfg.CreateMap<IntentResponseParameters, IntentResponseParameterModel>().ReverseMap();
                cfg.CreateMap<Entities, EntityModel>().ReverseMap();
                cfg.CreateMap<EntityEntries, EntityEntryModel>().ReverseMap();
            });
        }
    }
}
