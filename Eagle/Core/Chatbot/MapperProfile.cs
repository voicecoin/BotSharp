using Apps.Chatbot.DomainModels;
using AutoMapper;
using Core.DbTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Chatbot
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
