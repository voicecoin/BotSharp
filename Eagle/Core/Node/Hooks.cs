using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Enums;
using Core.Bundle;
using Microsoft.Extensions.Configuration;

namespace Core.Node
{
    public class Hooks : IHookDbInitializer
    {
        public int Priority => 1;
        public void Load(IHostingEnvironment env, IConfigurationRoot config, CoreDbContext dc)
        {
            if (dc.Table<BundleEntity>().Any(x => x.EntityName == "Node")) return;

            var dm = new DomainModel<BundleEntity>(dc, new BundleEntity { Name = "Client", EntityName = "Node" });
            dm.Add(dc);

            // Add fields
            dm.AddField(dc, new BundleFieldEntity { BundleId = dm.Entity.Id, FieldTypeId = FieldTypes.Text, Name = "Client Code" });
            dm.AddField(dc, new BundleFieldEntity { BundleId = dm.Entity.Id, FieldTypeId = FieldTypes.Boolean, Name = "Enable Service" });

            dc.SaveChanges();
        }
    }
}
