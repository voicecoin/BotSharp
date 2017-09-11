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
        public static bool Add(this DomainModel<BundleEntity> bundleModel, CoreDbContext dc)
        {
            return bundleModel.AddEntity();
        }

        public static bool AddField(this DomainModel<BundleEntity> bundleModel, CoreDbContext dc, BundleFieldEntity bundleFieldModel)
        {
            var dm = new DomainModel<BundleFieldEntity>(dc, bundleFieldModel);
            return dm.AddEntity();
        }
    }
}
