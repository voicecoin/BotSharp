using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.DbTables;

namespace Eagle.Core.Menu
{
    public class VmMenu
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public String Link { get; set; }
        public String DataUrl { get; set; }
        public List<VmMenu> Items { get; set; }
    }
}
