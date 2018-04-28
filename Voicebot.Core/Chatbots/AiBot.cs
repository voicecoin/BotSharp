using BotSharp.Core.Agents;
using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.Core.Chatbots
{
    public class AiBot
    {
        public string CreateAgent(Database dc, Agent agent)
        {
            agent.ClientAccessToken = Guid.NewGuid().ToString("N");
            agent.DeveloperAccessToken = Guid.NewGuid().ToString("N");

            dc.Table<Agent>().Add(agent);

            return agent.Id;
        }
    }
}
