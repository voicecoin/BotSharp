using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Interfaces;

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
                        select new
                        {
                            agent.Name,
                            agent.Avatar,
                            IsAlly = ally != null,
                            SkillId = agent.Id
                        };

            return query.ToList();
        }

        [HttpPost("{agentId}/{skillId}")]
        public bool AddSkill(String agentId, String skillId)
        {
            var skill = dc.Table<AgentSkillEntity>().FirstOrDefault(x => x.AgentId == agentId && x.SkillId == skillId);
            if (skill != null) return true;

            dc.Transaction<IDbRecord4Core>(delegate {
                var dm = new DomainModel<AgentSkillEntity>(dc, new AgentSkillEntity
                {
                    AgentId = agentId,
                    SkillId = skillId
                });

                dm.AddEntity();
            });

            return true;
        }

        [HttpDelete("{agentId}/{skillId}")]
        public bool RemoveSkill(String agentId, String skillId)
        {
            dc.Transaction<IDbRecord4Core>(delegate {

                var skill = dc.Table<AgentSkillEntity>().FirstOrDefault(x => x.AgentId == agentId && x.SkillId == skillId);
                if(skill != null)
                {
                    dc.Table<AgentSkillEntity>().Remove(skill);
                }

            });

            return true;
        }
    }
}
