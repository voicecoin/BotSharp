using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BotSharp.Core.Agents;
using BotSharp.Core.Engines;
using EntityFrameworkCore.BootKit;
using Voicebot.Core.Interfaces;

namespace Voicebot.Core.Chatbots
{
    public class AgentDbInitializer : IHookDbInitializer
    {
        public int Priority => 10;

        public void Load(Database dc)
        {
            if (!dc.Table<Agent>().Any(x => x.Id == AiBot.BUILTIN_ZH_BOT_ID))
            {
                dc.Table<Agent>().Add(new Agent {
                    Id = AiBot.BUILTIN_ZH_BOT_ID,
                    Name = "内建机器人",
                    UserId = AiBot.BUILTIN_USER_ID,
                    Language = "zh"
                });
                dc.SaveChanges();
            }

            if (!dc.Table<Agent>().Any(x => x.Id == "f4811cf8-0eb8-4eb3-8271-fe2bc6da27f3"))
            {
                ImportVoicebot(dc);
                dc.SaveChanges();
            }

            if (!dc.Table<Agent>().Any(x => x.Id == "b2ca5be5-72f0-4a91-83b6-3bab99dc0810"))
            {
                ImportYayabot(dc);
                dc.SaveChanges();
            }
        }

        private void ImportVoicebot(Database dc)
        {
            var rasa = new RasaAi(dc);
            var importer = new AgentImporterInDialogflow();

            string dataDir = $"{Database.ContentRootPath}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Agents";
            var agent = rasa.RestoreAgent(importer, "Voicebot", dataDir);
            agent.Id = "f4811cf8-0eb8-4eb3-8271-fe2bc6da27f3";
            agent.UserId = "8da9e1e0-42dc-420a-8016-79b04c1297d0";
            agent.ClientAccessToken = "d018bf12a8a8419797fe3965637389b0";
            agent.DeveloperAccessToken = "8553e861eecd4cd7a1c6aff6bdd1cd2f";
            rasa.agent = agent;

            rasa.agent.SaveAgent(dc);
        }

        private void ImportYayabot(Database dc)
        {
            var rasa = new RasaAi(dc);
            var importer = new AgentImporterInDialogflow();

            string dataDir = $"{Database.ContentRootPath}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Agents";
            var agent = rasa.RestoreAgent(importer, "Yayabot", dataDir);
            agent.Id = "b2ca5be5-72f0-4a91-83b6-3bab99dc0810";
            agent.UserId = "8da9e1e0-42dc-420a-8016-79b04c1297d0";
            agent.ClientAccessToken = "50dbb57981654aa1a6bbf24f612f207f";
            agent.DeveloperAccessToken = "da38d1468cde461b8643f899c680a26b";
            rasa.agent = agent;

            rasa.agent.SaveAgent(dc);
        }
    }
}
