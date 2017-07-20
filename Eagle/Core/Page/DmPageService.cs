using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Core.Page
{
    public static class DmPageService
    {
        public static void Add(this DmPage pageModel, CoreDbContext dc)
        {
            if (dc.Table<PageEntity>().Any(x => x.Name.Equals(pageModel.Name))) return;

            var dbRecord = pageModel.Map<PageEntity>();

            dbRecord.CreatedUserId = dc.CurrentUser.Id;
            dbRecord.CreatedDate = DateTime.UtcNow;
            dbRecord.ModifiedUserId = dc.CurrentUser.Id;
            dbRecord.ModifiedDate = DateTime.UtcNow;

            dc.Table<PageEntity>().Add(dbRecord);
        }
    }
}
