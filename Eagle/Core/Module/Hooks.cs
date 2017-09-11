using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Menu;

namespace Core.Module
{
    public class Hooks : IHookMenu
    {
        public void UpdateMenu(List<VmMenu> menus, CoreDbContext dc)
        {
            var menu = menus.Find(x => x.Name == "Configuration");

            var list = new VmMenu
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Modules",
                Link = "/Shared/Page/",
                Icon = "grid"
            };

            menu.Items.Add(list);
        }
    }
}
