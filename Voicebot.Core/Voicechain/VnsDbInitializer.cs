using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using Voicebot.Core.Interfaces;

namespace Voicebot.Core.Voicechain
{
    public class VnsDbInitializer : IHookDbInitializer
    {
        public int Priority => 100;

        public void Load(Database dc)
        {
            string dataDir = $"{Database.ContentRootPath}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Voiceweb{Path.DirectorySeparatorChar}Vns.json";
            string json = File.ReadAllText(dataDir);
            var vns = JsonConvert.DeserializeObject<List<Data>>(json);

            if (!dc.Table<VnsTable>().Any())
            {
                dc.Table<VnsTable>().AddRange(vns.Select(x => new VnsTable
                {
                    Name = x.Name,
                    Domain = x.Value,
                    AgentId = x.AgentId
                }));
            }
        }

        public class Data
        {
            public String Name { get; set; }
            public String Value { get; set; }
            public String AgentId { get; set; }
        }
    }
}
