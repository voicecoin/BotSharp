using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.DbTables;
using Eagle.DomainModels;
using Eagle.Apps.Chatbot.DomainModels;

namespace Eagle.Apps.Chatbot
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Agents, DmAgent>().ReverseMap();
            CreateMap<Intents, DmIntent>().ReverseMap();
            CreateMap<IntentExpressions, DmIntentExpression>().ReverseMap();
            CreateMap<IntentResponses, DmIntentResponse>().ReverseMap();
            CreateMap<IntentResponseMessages, DmIntentResponseMessage>().ReverseMap();
            CreateMap<IntentResponseParameters, DmIntentResponseParameter>().ReverseMap();
            CreateMap<Entities, DmEntity>().ReverseMap();
            CreateMap<EntityEntries, DmEntityEntry>().ReverseMap();
        }
    }
}
