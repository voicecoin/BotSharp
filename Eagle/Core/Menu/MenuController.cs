using Core.Interfaces;
using DotNetToolkit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

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
                Icon = "ios-keypad-outline",
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
                Icon = "hammer",
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
                Link = "/Structure/Pages",
                Icon = "file"
            };

            structure.Items.Add(pages);

            /*var blocks = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Blocks",
                Link = "/Structure/Blocks"
            };

            structure.Items.Add(blocks);

            var views = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Views",
                Link = "/Structure/Views"
            };

            structure.Items.Add(views);*/

            var config = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Configuration",
                Icon = "settings",
                Items = new List<VmMenu>()
            };

            var workflow = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Workflow",
                Icon = "cloud",
                Items = new List<VmMenu>()
            };

            // Level 1 menus
            menu.Add(workflow);

            var rules = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Rules",
                Link = "/Structure/Rules"
            };

            workflow.Items.Add(rules);

            var Scene = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Scenes",
                Link = "/Structure/Scenes"
            };

            workflow.Items.Add(Scene);

            // Level 1 menus
            menu.Add(config);

            var site = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Basic Site Settings",
                Link = "/Configuration/Site-Information"
            };

            config.Items.Add(site);

            var docs = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Document",
                Icon = "book"
            };

            // Level 1 menus
            menu.Add(docs);

            TypeHelper.GetInstanceWithInterface<IHookMenu>("Core", "Apps").ForEach(m => m.UpdateMenu(menu, dc));

            return Ok(menu);
        }
    }
}
