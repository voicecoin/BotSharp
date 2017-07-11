using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DomainModels
{
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
}
