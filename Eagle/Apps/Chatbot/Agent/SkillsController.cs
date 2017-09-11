using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Apps.Chatbot.Agent
{
    public class SkillsController : CoreController
    {
        [HttpGet("{agentId}")]
        public IEnumerable<Object> GetAlignments(String agentId)
        {
            var query = from agent in dc.Table<AgentEntity>()
                        from ally in dc.Table<AgentSkillEntity>().Where(x => x.AgentId == agentId && x.SkillId == agent.Id).DefaultIfEmpty()
                        where agent.IsSkillset && agent.Id != agentId
                        select new { agent.Name, agent.Avatar, IsAlly = ally != null };

            return query.ToList();
        }
    }
}
