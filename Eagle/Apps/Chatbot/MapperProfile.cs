using Apps.Chatbot.Agent;
using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Entity;
using Apps.Chatbot.Intent;
using AutoMapper;
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
            CreateMap<AgentEntity, DmAgent>().ReverseMap();
            CreateMap<IntentEntity, DmIntent>().ReverseMap();
            CreateMap<IntentExpressionEntity, DmIntentExpression>().ReverseMap();
            CreateMap<IntentResponseEntity, DmIntentResponse>().ReverseMap();
            CreateMap<IntentResponseMessageEntity, DmIntentResponseMessage>().ReverseMap();
            CreateMap<IntentResponseParameterEntity, DmIntentResponseParameter>().ReverseMap();
            CreateMap<EntityEntity, DmEntity>().ReverseMap();
            CreateMap<EntityEntryEntity, DmEntityEntry>().ReverseMap();
        }
    }
}
