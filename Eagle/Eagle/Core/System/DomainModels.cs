using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Core
{
    public class DomainModel
    {

    }

    public class DmQuery<T>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public T Data { get; set; }
    }

    public class DmPageResult<T>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
        public List<T> Items { get; set; }
    }

    public class Constants
    {
        public static readonly string SystemUserId = "28d2bf0c-c84f-4b63-b0d7-f1a459a09ade";
    }

}
