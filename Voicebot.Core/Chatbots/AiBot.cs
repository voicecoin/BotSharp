using BotSharp.Core.Agents;
using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voicebot.Core.Chatbots
{
    public class AiBot
    {
        /// <summary>
        /// 内建中文机器人
        /// </summary>
        public const string BUILTIN_ZH_BOT_ID = "fda1f52e-c9f0-4acc-8c21-80e4f867f50a";
        public const string BUILTIN_USER_ID = "e1c81d18-c2fa-4165-b9bd-ad9d47c048b3";

        public string CreateAgent(Database dc, Agent agent)
        {
            agent.ClientAccessToken = Guid.NewGuid().ToString("N");
            agent.DeveloperAccessToken = Guid.NewGuid().ToString("N");

            dc.Table<Agent>().Add(agent);

            return agent.Id;
        }
    }
}
