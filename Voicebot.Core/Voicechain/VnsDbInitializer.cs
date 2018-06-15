using System;
using System.Collections.Generic;
using System.Text;
using EntityFrameworkCore.BootKit;
using Voicebot.Core.Interfaces;

namespace Voicebot.Core.Voicechain
{
    public class VnsDbInitializer : IHookDbInitializer
    {
        public int Priority => 100;

        public void Load(Database dc)
        {
            dc.Table<VnsTable>().Add(new VnsTable
            {
                Name = "apple store",
                Domain = "music.bot"
            });
        }
    }
}
