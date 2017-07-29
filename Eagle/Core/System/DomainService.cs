using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public static class DomainService
    {
        public static DmPageResult<T> LoadDataByPage<T>(this DmPageResult<T> dmPage, IQueryable<T> query)
        {
            int page = dmPage.Page;
            int size = dmPage.Size;

            var total = query.Count();
            var items = query.Skip((page - 1) * size).Take(size).ToList();

            return new DmPageResult<T> { Total = total, Page = page, Size = size, Items = items };
        }

    }
}
