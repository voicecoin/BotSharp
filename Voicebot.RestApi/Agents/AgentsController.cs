using BotSharp.Core.Agents;
using BotSharp.Core.Engines;
using BotSharp.Core.Models;
using DotNetToolkit;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voicebot.Core.Chatbots;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Agent controller
    /// </summary>
    public class AgentsController : CoreController
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
            agent.UserId = CurrentUserId;

            dc.DbTran(() =>
            {
                agentId = bot.CreateAgent(dc, agent.ToObject<Agent>());
            });

            return agentId;
        }

        /// <summary>
        /// Delete a existed agent permanently
        /// </summary>
        /// <param name="agentId"></param>
        [HttpDelete]
        public void Delete([FromRoute] string agentId)
        {

        }

        /// <summary>
        /// Update existed agent
        /// </summary>
        /// <param name="agent"></param>
        [HttpPut]
        public void Update([FromBody] VmAgent agent)
        {

        }

        [HttpGet("{agentId}")]
        public Object GetAgentDetail([FromRoute] string agentId)
        {
            var agent = dc.Table<Agent>().Find(agentId);

            return agent;
        }

        [HttpGet("MyAgents")]
        public PageResult<VmAgent> MyAgents()
        {
            var result = new PageResult<VmAgent>() { };
            var query = dc.Table<Agent>().Where(x => x.UserId == CurrentUserId).Select(x => x.ToObject<VmAgent>());
            return result.LoadRecords<VmAgent>(query);
        }

        [HttpGet("{userId}/query")]
        public PageResult<Object> Query([FromRoute] string userId)
        {
            var result = new PageResult<Object>() { };
            var query = dc.Table<Agent>().Where(x => x.UserId == userId);
            return result.LoadRecords<Object>(query);
        }

        [AllowAnonymous]
        [HttpGet("{agentId}/restore")]
        public string Restore([FromRoute] string agentId)
        {
            var rasa = new RasaAi(dc);
            rasa.agent = rasa.LoadAgentById(dc, agentId);
            return rasa.Train();
        }

        [AllowAnonymous]
        [HttpGet("{agentId}/train")]
        public string Train([FromRoute] string agentId)
        {
            var rasa = new RasaAi(dc);
            rasa.agent = rasa.LoadAgentById(dc, agentId);
            return rasa.Train();
        }
    }
}
