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
        public static void Add(this DomainModel<BundleFieldEntity> bundleFieldModel, CoreDbContext dc)
        {
            bundleFieldModel.AddEntity();
        }
    }
}
