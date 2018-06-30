using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using Voicebot.Core.Interfaces;

namespace Voicebot.Core.Chatbots.SkillSets
{
    public class SkillSetDbInitializer : IHookDbInitializer
    {
        public int Priority => 300;

        public void Load(Database dc)
        {
            //string dataDir = $"{Database.ContentRootPath}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}SkillSets{Path.DirectorySeparatorChar}skillsets.json";
            //var skillSets = JsonConvert.DeserializeObject<List<SkillSetOfAgent>>(File.ReadAllText(dataDir));
        }

        private void ImportSkillSet()
        {

        }
    }
}
