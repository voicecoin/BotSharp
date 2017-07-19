using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.DbTables;
using Core.DataContexts;
using Core.Enums;
using Core.DomainModels;
using Core.Bundle;

namespace Core.Node
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 1;
        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            if (dc.Bundles.Any(x => x.EntityName == "Node")) return;

            DmBundle bundle = new DmBundle { Name = "Client", EntityName = "Node" };
            bundle.Add(dc);

            // Add fields
            bundle.AddField(dc, new DmBundleField { BundleId = bundle.Id, FieldTypeId = FieldTypes.Text, Name = "Client Code" });
            bundle.AddField(dc, new DmBundleField { BundleId = bundle.Id, FieldTypeId = FieldTypes.Boolean, Name = "Enable Service" });

            dc.SaveChanges();
        }
    }
}
