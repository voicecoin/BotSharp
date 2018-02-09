using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core;
using Utility;
using Core.Interfaces;
using System.Collections.Generic;
using EntityFrameworkCore.BootKit;
using DotNetToolkit;

namespace Apps.Chatbot.Agent
{
    public class AgentsController : CoreController
    {
        // GET: v1/Agents
        [HttpGet("Query")]
        public PageResult<AgentEntity> GetAllAgents(string name, [FromQuery] int page = 1)
        {
            var query = dc.Table<AgentEntity>().AsQueryable();
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            var total = query.Count();

            int size = 20;

            var items = query.Skip((page - 1) * size).Take(size).Select(x => x.Map<AgentEntity>()).ToList();
            return new PageResult<AgentEntity> { Total = total, Page = page, Size = size, Items = items };
        }

        [HttpGet("MyAgents")]
        public IEnumerable<Object> MyAgents()
        {
            var user = GetCurrentUser();
            return dc.Table<AgentEntity>().Select(x => new { x.Name });
        }

        // GET: v1/Agents
        [HttpGet("{userId}/Query")]
        public PageResult<AgentEntity> GetAgents([FromRoute] string userId, string name, [FromQuery] int page = 1)
        {
            var query = dc.Table<AgentEntity>();

            var total = query.Count();

            int size = 20;

            var items = query.Skip((page - 1) * size).Take(size).Select(x => x.Map<AgentEntity>()).ToList();
            return new PageResult<AgentEntity> { Total = total, Page = page, Size = size, Items = items };
        }

        // GET: v1/Agents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgent([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agents = dc.Table<AgentEntity>().FirstOrDefault();

            if (agents == null)
            {
                return NotFound();
            }

            var agent = agents.Map<AgentEntity>();

            return Ok(agent);
        }

        [HttpGet]
        public async Task<IActionResult> InitAgent()
        {
            var intent = new AgentEntity()
            {
                Name = "未命名机器人",
                Language = "zh-cn",
                ClientAccessToken = Guid.NewGuid().ToString("N"),
                DeveloperAccessToken = Guid.NewGuid().ToString("N")
            };

            return Ok(intent);
        }

        // PUT: v1/Agents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgents([FromRoute] string id, [FromBody] AgentEntity agentModel)
        {
            dc.Transaction<IDbRecord>(delegate {
                var agentRecord = dc.Table<AgentEntity>().Find(id);

                agentRecord.Name = agentModel.Name;
                agentRecord.Description = agentModel.Description;
                //agentRecord.Language = agentModel.Language;
                if (!String.IsNullOrEmpty(agentModel.Avatar))
                {
                    agentRecord.Avatar = agentModel.Avatar;
                }
                agentRecord.IsPublic = agentModel.IsPublic;
            });

            return Ok();
        }

        // POST: v1/Agents
        [HttpPost]
        public async Task<IActionResult> PostAgent([FromBody] AgentEntity agentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        // DELETE: v1/Agents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgents([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agents = dc.Table<AgentEntity>().FirstOrDefault();
            if (agents == null)
            {
                return NotFound();
            }

            dc.Table<AgentEntity>().Remove(agents);
            dc.SaveChanges();

            return Ok(agents);
        }
    }
}