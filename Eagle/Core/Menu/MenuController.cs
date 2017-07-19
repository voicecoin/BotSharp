using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Menu
{
    public class MenuController : CoreController
    {
        [HttpGet]
        public async Task<IActionResult> GetMenus()
        {
            var menu = new List<VmMenu>();

            var dashboard = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Dashboard",
                Icon = "appstore",
                Link = "/Dashboard",
                Items = new List<VmMenu>()
            };

            // Level 1 menus
            menu.Add(dashboard);

            var content = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Content",
                Icon = "edit",
                Items = new List<VmMenu>()
            };

            // Level 1 menus
            menu.Add(content);

            var records = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Records",
                Icon = "edit",
                Link = "/Structure/Record"
            };

            content.Items.Add(records);

            var structure = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Structure",
                Icon = "mail",
                Items = new List<VmMenu>()
            };

            // Level 1 menus
            menu.Add(structure);

            var bundle = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Bundles",
                Link = "/Structure/Bundles"
            };

            structure.Items.Add(bundle);

            var taxonomy = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Taxonomy",
                Link = "/Structure/Taxonomy"
            };

            structure.Items.Add(taxonomy);

            var pages = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Pages",
                Link = "/Structure/Pages"
            };

            structure.Items.Add(pages);

            var views = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Views",
                Link = "/Structure/Views"
            };

            structure.Items.Add(views);

            var rules = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Rules",
                Link = "/Structure/Rules"
            };

            structure.Items.Add(rules);

            var config = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Configuration",
                Icon = "setting",
                Items = new List<VmMenu>()
            };

            // Level 1 menus
            menu.Add(config);

            var site = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Basic Site Settings",
                Link = "/Configuration/Site-Information"
            };

            config.Items.Add(site);

            var userList = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Users",
                Link = "/Shared/Page/1"
            };

            config.Items.Add(userList);

            var userProfile = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "My Profile",
                Link = "/Configuration/People/Profile"
            };

            config.Items.Add(userProfile);

            var docs = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Document",
                Icon = "mail"
            };

            // Level 1 menus
            menu.Add(docs);

            return Ok(menu);
        }
    }
}
