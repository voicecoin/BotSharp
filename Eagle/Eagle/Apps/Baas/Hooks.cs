using Eagle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.DataContexts;
using Microsoft.AspNetCore.Hosting;
using Eagle.DbTables;
using Eagle.Enums;
using Eagle.Core.Interfaces;

namespace Eagle.Apps.Baas
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 1000;

        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            var bundle = dc.Bundles.FirstOrDefault(x => x.EntityName == "User");

            var rootUser = dc.Users.Find("8a9fd693-9038-4083-87f7-08e45eff61d2");
            if (rootUser == null)//means app need create an root user
            {
                UserEntity accountModel = new UserEntity();
                accountModel.Id = "8a9fd693-9038-4083-87f7-08e45eff61d2";
                accountModel.BundleId = bundle.Id;
                accountModel.CreatedUserId = accountModel.Id;
                accountModel.CreatedDate = DateTime.UtcNow;
                accountModel.ModifiedUserId = accountModel.Id;
                accountModel.ModifiedDate = DateTime.UtcNow;
                accountModel.UserName = "info@yaya.ai";
                accountModel.FirstName = "Yaya";
                accountModel.LastName = "Bot";
                accountModel.Email = "info@yaya.ai";
                accountModel.Password = "Yayabot123";
                accountModel.Description = "丫丫人工智能聊天机器人";
                dc.Users.Add(accountModel);
            }

            rootUser = dc.Users.Find("265d804d-0073-4a50-bd07-98a28e10f9fb");
            if (rootUser == null)
            {
                UserEntity accountModel = new UserEntity();
                accountModel.Id = "265d804d-0073-4a50-bd07-98a28e10f9fb";
                accountModel.BundleId = bundle.Id;
                accountModel.CreatedUserId = accountModel.Id;
                accountModel.CreatedDate = DateTime.UtcNow;
                accountModel.ModifiedUserId = accountModel.Id;
                accountModel.ModifiedDate = DateTime.UtcNow;
                accountModel.UserName = "yrdrylcyp@163.com";
                accountModel.FirstName = "灵溪山谷";
                accountModel.Email = "yrdrylcyp@163.com";
                accountModel.Password = "Yayabot123";
                accountModel.Description = "鹰潭东瑞实业有限公司";
                dc.Users.Add(accountModel);
            }

            dc.SaveChanges();
        }
    }
}
