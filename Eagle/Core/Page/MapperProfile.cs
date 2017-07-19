using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DbTables;
using Core.DomainModels;

namespace Core.Page
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DmPage, PageEntity>().ReverseMap();
        }
    }
}
