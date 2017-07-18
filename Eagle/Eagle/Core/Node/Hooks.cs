using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Core.Interfaces;
using Eagle.DataContexts;
using Eagle.DbTables;
using Eagle.Enums;

namespace Eagle.Core.Node
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 1;
        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            if (dc.Bundles.Any(x => x.EntityName == "Node")) return;

            BundleEntity bundle = new BundleEntity { Name = "Client", EntityName = "Node", Status = EntityStatus.Active };
            dc.Bundles.Add(bundle);
            dc.SaveChanges();

            // Add fields
            dc.BundleFields.Add(new BundleFieldEntity { BundleId = bundle.Id, FieldTypeId = FieldTypes.Text, Name = "Client Code", Status = EntityStatus.Active });
            dc.BundleFields.Add(new BundleFieldEntity { BundleId = bundle.Id, FieldTypeId = FieldTypes.Boolean, Name = "Enable Service", Status = EntityStatus.Active });

            dc.SaveChanges();
        }
    }
}
