using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Core.DbTables;
using Core.Interfaces;
using Core.DataContexts;
using Core.Account;

namespace Apps.Baas
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 1000;

        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            var bundle = dc.Bundles.FirstOrDefault(x => x.EntityName == "User");

            DmAccount accountModel = new DmAccount
            {
                Id = "8a9fd693-9038-4083-87f7-08e45eff61d2",
                BundleId = bundle.Id,
                UserName = "info@yaya.ai",
                FirstName = "Yaya",
                LastName = "Bot",
                Email = "info@yaya.ai",
                Password = "Yayabot123",
                Description = "丫丫人工智能聊天机器人"
            };
            accountModel.Add(dc);

            accountModel = new DmAccount
            {
                Id = "265d804d-0073-4a50-bd07-98a28e10f9fb",
                BundleId = bundle.Id,
                UserName = "yrdrylcyp@163.com",
                FirstName = "灵溪山谷",
                Email = "yrdrylcyp@163.com",
                Password = "Yayabot123",
                Description = "鹰潭东瑞实业有限公司"
            };
            accountModel.Add(dc);
        }
    }
}
