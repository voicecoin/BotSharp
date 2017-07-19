using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Core.Interfaces;
using Core.System;
using Core.DataContexts;
using Core.DbTables;

namespace Core.Account
{
    public static class DmAccountService
    {
        public static void Add(this DmAccount accountModel, CoreDbContext dc)
        {
            if (dc.Users.Any(x => x.UserName.Equals(accountModel.UserName))) return;

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
