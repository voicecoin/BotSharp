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
            CreateMap<EntityEntity, DmEntity>().ReverseMap();
            CreateMap<EntityEntryEntity, DmEntityEntry>().ReverseMap();
        }
    }
}
