using Core.DomainModels;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Core.Bundle
{
    public static class DmBundleFieldService
    {
        public static void Add(this DmBundleField bundleFieldModel, CoreDbContext dc)
        {
            if (dc.Table<BundleFieldEntity>().Any(x => x.Name == bundleFieldModel.Name && x.BundleId == bundleFieldModel.BundleId)) return;

            var dbRecord = bundleFieldModel.Map<BundleFieldEntity>();

            dbRecord.Status = EntityStatus.Active;
            dbRecord.CreatedUserId = dc.CurrentUser.Id;
            dbRecord.CreatedDate = DateTime.UtcNow;
            dbRecord.ModifiedUserId = dc.CurrentUser.Id;
            dbRecord.ModifiedDate = DateTime.UtcNow;

            dc.Table<BundleFieldEntity>().Add(dbRecord);
        }
    }
}
