using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apps.Chatbot.Integration
{
    public class IntegrationController : CoreController
    {
        [HttpGet("{agentId}")]
        public IEnumerable<AgentPlatformEntity> GetAgentPlatforms(string agentId)
        {
            var platforms = dc.Table<AgentPlatformEntity>().Where(x => x.AgentId == agentId);
            return platforms;
        }
    }
}
