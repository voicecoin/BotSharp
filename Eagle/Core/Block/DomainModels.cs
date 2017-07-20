using Core.Interfaces;
using Core.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Block
{
    public class DmBlock : IDomainModel
    {
        public DmBlock()
        {
            Id = Guid.NewGuid().ToString();
            Menus = new List<KeyValuePair<string, string>>();
            Position = new DmBlockPosition();
        }

        public String Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String DataUrl { get; set; }

        public int Priority { get; set; } 

        public DmBlockPosition Position { get; set; }
        public List<KeyValuePair<String, String>> Menus { get; set; }
    }

    public class DmBlockPosition
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
