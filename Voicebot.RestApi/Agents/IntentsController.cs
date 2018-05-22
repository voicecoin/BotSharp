using BotSharp.Core.Agents;
using BotSharp.Core.Engines;
using BotSharp.Core.Intents;
using DotNetToolkit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.RestApi.Agents
{
    /// <summary>
    /// Intent controller
    /// </summary>
    public class IntentsController : CoreController
    {
        /// <summary>
        /// Create a intent for agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="intent"></param>
        /// <returns></returns>
        [HttpPost]
        public string CreateIntent([FromRoute] string agentId, [FromBody] VmIntentDetail intent)
        {
            var agent = new RasaAi(dc).LoadAgentById(dc, agentId);
            return agent.CreateIntent(dc, intent.ToObject<Intent>());
        }

        /// <summary>
        /// List all intents for specific agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        [HttpGet("{agentId}")]
        public PageResult<VmIntent> MyIntents([FromRoute] string agentId)
        {
            var result = new PageResult<VmIntent>() { };
            var query = dc.Table<Intent>().Where(x => x.AgentId == agentId).Select(x => x.ToObject<VmIntent>());
            return result.LoadRecords<VmIntent>(query);
        }

        /// <summary>
        /// Delete intent
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="intentId"></param>
        [HttpDelete("{agentId}/{intentId}")]
        public void DeleteIntent([FromRoute] string agentId, [FromRoute] string intentId)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intentId"></param>
        /// <param name="intent"></param>
        [HttpPut("{intentId}")]
        public void UpdateIntent([FromRoute] string intentId, [FromBody] VmIntentDetail intent)
        {

        }
    }
}
