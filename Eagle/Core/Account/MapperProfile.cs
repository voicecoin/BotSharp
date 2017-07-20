using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels;

namespace Core.Account
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DmAccount, UserEntity>().ReverseMap();
        }
    }
}
