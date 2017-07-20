using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DomainModels;

namespace Core.Bundle
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DmBundle, BundleEntity>().ReverseMap();
            CreateMap<DmBundleField, BundleFieldEntity>().ReverseMap();
        }
    }
}
