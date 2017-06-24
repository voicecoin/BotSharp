using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Utility
{
    public static class ObjectExtensions
    {
        public static T Map<T>(this Object source)
        {
            return Mapper.Map<T>(source);
        }
    }
}
