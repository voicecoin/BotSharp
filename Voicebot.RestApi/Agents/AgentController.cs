using BotSharp.Core.Agents;
using DotNetToolkit;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Voicebot.Core.Chatbots;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Agent controller
    /// </summary>
    public class AgentController : CoreController
    {
        /// <summary>
        /// Create agent with basic information
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        [HttpPost]
        public string Create([FromBody] VmAgent agent)
        {
            var bot = new AiBot();
            string agentId = String.Empty;

            dc.DbTran(() =>
            {
                agentId = bot.CreateAgent(dc, agent.ToObject<Agent>());
            });

            return agentId;
        }
    }
}
