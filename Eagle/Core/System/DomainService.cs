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

            dmPage.Total = query.Count();
            dmPage.Items = query.Skip((page - 1) * size).Take(size).ToList();

            return dmPage;
        }

    }
}
