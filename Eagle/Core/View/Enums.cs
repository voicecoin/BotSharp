using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.View
{
    public enum RepresentType
    {
        Grid = 1,
        Table = 2,
        Html = 3,
        List = 4
    }

    public enum DataContainer
    {
        Empty = 0,
        SeflHost = 1,
        RestApi = 2
    }
}
