using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotSharp.Core.Entities;
using EntityFrameworkCore.BootKit;
using Voicebot.Core.Interfaces;

namespace Voicebot.Core.Chatbots
{
    public class EntityTypeDbInitializer : IHookDbInitializer
    {
        public int Priority => 20;

        public void Load(Database dc)
        {
            if (!dc.Table<EntityType>().Any(x => x.Id == "52099c60-6656-4979-9940-dd24f052991c"))
            {
                dc.Table<EntityType>().AddRange(
                    new EntityType { AgentId = AiBot.BUILTIN_ZH_BOT_ID, Color = "9448db", Name = "著名人名", Id = "52099c60-6656-4979-9940-dd24f052991c" },
                    new EntityType { AgentId = AiBot.BUILTIN_ZH_BOT_ID, Color = "6953e7", Name = "国家", Id = "50662e1a-8bde-49e7-ae0b-d22e53eacafc" },
                    new EntityType { AgentId = AiBot.BUILTIN_ZH_BOT_ID, Color = "695300", Name = "动物", Id = "96a43388-dbea-44d0-803a-bface3becc39" }
                );
            }
        }
    }
}
