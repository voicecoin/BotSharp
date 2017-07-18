using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Core.Interfaces;
using Eagle.Core.System;
using Eagle.DataContexts;
using Eagle.DbTables;
using Eagle.Utility;
using Eagle.DomainModels;

namespace Eagle.Core.Account
{
    public static class DmAccountService
    {
        public static void Add(this DmAccount accountModel, CoreDbContext dc)
        {
            var bundle = dc.Bundles.First(x => x.EntityName == "User");

            var dbRecord = accountModel.Map<UserEntity>();

            dbRecord.BundleId = bundle.Id;
            dbRecord.CreatedUserId = dc.CurrentUser.Id;
            dbRecord.CreatedDate = DateTime.UtcNow;
            dbRecord.ModifiedUserId = dc.CurrentUser.Id;
            dbRecord.ModifiedDate = DateTime.UtcNow;

            dc.Users.Add(dbRecord);
        }
    }
}
