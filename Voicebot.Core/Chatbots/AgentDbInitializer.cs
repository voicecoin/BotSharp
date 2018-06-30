using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Amazon.Polly;
using BotSharp.Core.Agents;
using BotSharp.Core.Engines;
using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using Voicebot.Core.Interfaces;
using Voicebot.Core.TextToSpeech;

namespace Voicebot.Core.Chatbots
{
    public class AgentDbInitializer : IHookDbInitializer
    {
        public int Priority => 10;

        public void Load(Database dc)
        {
            string dataDir = $"{Database.ContentRootPath}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Agents{Path.DirectorySeparatorChar}agents.json";
            var agents = JsonConvert.DeserializeObject<List<AgentMeta>>(File.ReadAllText(dataDir));

            agents.ForEach(meta =>
            {
                Console.WriteLine($"Initializing {meta.Name}");
                if (!dc.Table<Agent>().Any(x => x.Id == meta.Id))
                {
                    ImportChatbot(dc, meta);
                    ImportAgentVoice(dc, meta);
                    dc.SaveChanges();
                }

            });

            if (!dc.Table<Agent>().Any(x => x.Id == AiBot.BUILTIN_ZH_BOT_ID))
            {
                dc.Table<Agent>().Add(new Agent {
                    Id = AiBot.BUILTIN_ZH_BOT_ID,
                    Name = "中文机器人",
                    Description = "系统内置中文辅助机器人",
                    UserId = AiBot.BUILTIN_USER_ID,
                    Language = "zh"
                });
                dc.SaveChanges();
            }
        }

        private void ImportChatbot(Database dc, AgentMeta meta)
        {
            var rasa = new RasaAi(dc);
            var importer = new AgentImporterInDialogflow();

            string dataDir = $"{Database.ContentRootPath}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Agents";
            var agent = rasa.RestoreAgent(importer, meta.Name, dataDir);
            agent.Id = meta.Id;
            agent.UserId = meta.UserId ?? AiBot.BUILTIN_USER_ID;
            agent.ClientAccessToken = meta.ClientAccessToken;
            agent.DeveloperAccessToken = meta.DeveloperAccessToken;
            rasa.agent = agent;

            rasa.agent.SaveAgent(dc);
        }

        private void ImportAgentVoice(Database dc, AgentMeta meta)
        {
            if(meta.VoiceId == null)
            {
                meta.VoiceId = VoiceId.Joanna;
            }

            var voice = new AgentVoice
            {
                VoiceId = meta.VoiceId,
                VoiceEngine = "Amazon Polly",
                AgentId = meta.Id
            };

            dc.Table<AgentVoice>().Add(voice);
        }


        private class AgentMeta
        {
            public string Id { get; set; }
            public string UserId { get; set; }
            public string Name { get; set; }
            public string ClientAccessToken { get; set; }
            public string DeveloperAccessToken { get; set; }
            public VoiceId VoiceId { get; set; }
        }
    }
}
