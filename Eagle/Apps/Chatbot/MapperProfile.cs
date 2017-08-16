using Apps.Chatbot_ConversationParameters.Agent;
using Apps.Chatbot_ConversationParameters.DomainModels;
using Apps.Chatbot_ConversationParameters.Entity;
using Apps.Chatbot_ConversationParameters.Intent;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Chatbot_ConversationParameters
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<EntityEntity, DmEntity>().ReverseMap();
            CreateMap<EntityEntryEntity, DmEntityEntry>().ReverseMap();
        }
    }
}
