using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.DataContexts;
using Core.DbTables;
using Core.Enums;
using Core.DomainModels;
using Core.Bundle;

namespace Core.Account
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 1;

        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            if (dc.Bundles.Any(x => x.EntityName == "User")) return;

            DmBundle bundle = new DmBundle { Name = "User Profile", EntityName = "User" };
            bundle.Add(dc);
        }
    }
}
