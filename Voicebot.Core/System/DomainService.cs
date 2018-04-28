using DotNetToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voicebot.Core
{
    public static class DomainService
    {
        public static PageResult<T> LoadDataByPage<T>(this PageResult<T> dmPage, IQueryable<T> query)
        {
            int page = dmPage.Page;
            int size = dmPage.Size;

            dmPage.Total = query.Count();
            dmPage.Items = query.Skip((page - 1) * size).Take(size).ToList();

            return dmPage;
        }

    }
}
