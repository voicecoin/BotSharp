using AutoMapper;
using Eagle.Chatbot.DomainModels;
using Eagle.DbTables;
using Eagle.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Core
{
    public class MapperInitializer
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Agents, DmAgent>().ReverseMap();
                cfg.CreateMap<Intents, DmIntent>().ReverseMap();
                cfg.CreateMap<IntentExpressions, DmIntentExpression>().ReverseMap();
                cfg.CreateMap<IntentExpressionItems, DmIntentExpressionItem>().ReverseMap();
                cfg.CreateMap<IntentResponses, DmIntentResponse>().ReverseMap();
                cfg.CreateMap<IntentResponseContexts, DmIntentResponseContext>().ReverseMap();
                cfg.CreateMap<IntentResponseMessages, DmIntentResponseMessage>().ReverseMap();
                cfg.CreateMap<IntentResponseParameters, DmIntentResponseParameter>().ReverseMap();
                cfg.CreateMap<Entities, DmEntity>().ReverseMap();
                cfg.CreateMap<EntityEntries, DmEntityEntry>().ReverseMap();
            });
        }
    }
}
