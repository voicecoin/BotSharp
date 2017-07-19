using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DbTables;
using DomainModels;

namespace Core.Node
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DmNode, NodeEntity>().ReverseMap();
        }
    }
}
