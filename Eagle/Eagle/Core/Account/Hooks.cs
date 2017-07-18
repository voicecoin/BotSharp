using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.DataContexts;
using Microsoft.AspNetCore.Hosting;
using Eagle.Core.Interfaces;
using Eagle.DbTables;
using Eagle.Enums;

namespace Eagle.Core.Account
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 1;

        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            if (dc.Bundles.Any(x => x.EntityName == "User")) return;

            BundleEntity bundle = dc.Bundles.Add(new BundleEntity { Name = "User Profile", EntityName = "User", Status = EntityStatus.Active }).Entity;
            dc.Bundles.Add(bundle);

            dc.SaveChanges();
        }
    }
}
