using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DbTables;
using DomainModels;
using Core.DbTables;

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
