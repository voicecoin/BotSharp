using Core.DataContexts;
using Core.DbTables;
using Core.DomainModels;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Core.Bundle
{
    public static class DmBundleService
    {
        public static void Add(this DmBundle bundleModel, CoreDbContext dc)
        {
            if (dc.Bundles.Any(x => x.Name == bundleModel.Name && x.EntityName == bundleModel.EntityName)) return;

            var dbRecord = bundleModel.Map<BundleEntity>();

            dbRecord.Status = EntityStatus.Active;
            dbRecord.CreatedUserId = dc.CurrentUser.Id;
            dbRecord.CreatedDate = DateTime.UtcNow;
            dbRecord.ModifiedUserId = dc.CurrentUser.Id;
            dbRecord.ModifiedDate = DateTime.UtcNow;

            dc.Bundles.Add(dbRecord);
        }

        public static void AddField(this DmBundle bundleModel, CoreDbContext dc, DmBundleField bundleFieldModel)
        {
            bundleFieldModel.Add(dc);
        }
    }
}
