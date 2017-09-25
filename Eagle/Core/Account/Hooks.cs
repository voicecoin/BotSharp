using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Bundle;
using Core.Menu;
using Core.Page;
using Core.Block;
using Core.View;
using Newtonsoft.Json;
using Core.Enums;
using Microsoft.Extensions.Configuration;

namespace Core.Account
{
    public class Hooks : IHookDbInitializer, IHookMenu
    {
        public int Priority => 1000;

        public void Load(IHostingEnvironment env, IConfiguration config, CoreDbContext dc)
        {
            InitRoles(dc);

            if (dc.Table<BundleEntity>().Any(x => x.EntityName == "User")) return;

            var dm = new DomainModel<BundleEntity>(dc, new BundleEntity { Name = "User Profile", EntityName = "User" });
            dm.Add(dc);

            // Create view
            var dmView = new DomainModel<ViewEntity>(dc, new ViewEntity
            {
                Name = "Users",
                RepresentType = RepresentType.Table,
                Columns = new List<ViewColumEntity>(),
                Actions = new List<ViewActionEntity>(),
                DataContainer = DataContainer.SeflHost
            });

            dmView.Entity.Columns.AddRange(new List<ViewColumEntity> {
                    new ViewColumEntity { DisplayName = "Avatar", FieldName = "avatar", FieldType = FieldTypes.Image },
                    new ViewColumEntity { DisplayName = "User Name", FieldName = "userName", FieldType = FieldTypes.Text },
                    new ViewColumEntity { DisplayName = "First Name", FieldName = "firstName", FieldType = FieldTypes.Text },
                    new ViewColumEntity { DisplayName = "Last Name", FieldName = "lastName", FieldType = FieldTypes.Text },
                    new ViewColumEntity { DisplayName = "Email", FieldName = "email", FieldType = FieldTypes.Text },
                    new ViewColumEntity { DisplayName = "Created Date", FieldName = "createdDate", FieldType = FieldTypes.DateTime },
                    new ViewColumEntity { DisplayName = "Status", FieldName = "status", FieldType = FieldTypes.Boolean }
                });

            dmView.Entity.Actions.AddRange(
                new List<ViewActionEntity> {
                    new ViewActionEntity { Name = "Edit", RedirectUrl = "/Configuration/People/Profile/{id}" },
                    new ViewActionEntity { Name = "Delete", RestApiPath = "/api/Account/{id}", RequestMethod = "DELETE" },
                    new ViewActionEntity { Name = "Export", RedirectUrl = "/api/Account", IsViewLevel = true }
                });

            DmViewService.Add(dmView);

            var dmBlock = new DomainModel<BlockEntity>(dc, new BlockEntity
            {
                ViewId = dmView.Entity.Id,
                Name = "Users",
                Priority = 1,
                Menus = new List<KeyValuePair<String, String>>() {
                    new KeyValuePair<string, string>("Menu1", "/"),
                    new KeyValuePair<string, string>("Menu1", "/")
                }
            });
            dmBlock.AddEntity();

            var dmPage = new DomainModel<PageEntity>(dc, new PageEntity
            {
                Name = "Users",
                Description = "List all authorized users.",
                UrlPath = "Users",
                Blocks = new List<BlockEntity> { dmBlock.Entity }
            });
            DmPageService.Add(dmPage);
        }

        private void InitRoles(CoreDbContext dc)
        {
            if (dc.Table<RoleEntity>().Count() > 0) return;

            var dm = new DomainModel<RoleEntity>(dc, new RoleEntity { Name = "Anonymous" });
            dm.AddEntity();

            dm = new DomainModel<RoleEntity>(dc, new RoleEntity { Name = "Authenticated" });
            dm.AddEntity();

            dm = new DomainModel<RoleEntity>(dc, new RoleEntity { Name = "Administrator" });
            dm.AddEntity();
        }

        public void UpdateMenu(List<VmMenu> menus, CoreDbContext dc)
        {
            var menu = menus.Find(x => x.Name == "Configuration");

            var userList = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Users",
                Link = "/Shared/Page/" + dc.Table<PageEntity>().First(x => x.Name == "Users").Id,
                Icon = "person-stalker"
            };

            menu.Items.Add(userList);
        }
    }
}
