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
            if (!dc.Table<Agent>().Any())
            {
                ImportVoicebot(dc);
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
            agent.ClientAccessToken = "4e53fdb43a254dcc9ffdf78c831606b1";
            agent.DeveloperAccessToken = "d339390777e044c893bb08b0ff5e6ea0";

            rasa.SaveAgent(agent);
        }
    }
}
