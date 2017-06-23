using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eagle.DbContexts;
using Eagle.DbTables;

namespace Eagle.Modules.Agent
{
    [Route("v1/Agents")]
    public class AgentsController : ControllerBase
    {
        private readonly DataContexts _context = new DataContexts();

        // GET: v1/Agents
        [HttpGet]
        public IEnumerable<Agents> GetAgents()
        {
            return _context.Agents;
        }

        // GET: v1/Agents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgents([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agents = await _context.Agents.SingleOrDefaultAsync(m => m.Id == id);

            if (agents == null)
            {
                return NotFound();
            }

            return Ok(agents);
        }

        // PUT: v1/Agents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgents([FromRoute] string id, [FromBody] Agents agents)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != agents.Id)
            {
                return BadRequest();
            }

            _context.Entry(agents).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: v1/Agents
        [HttpPost]
        public async Task<IActionResult> PostAgents([FromBody] Agents agents)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            agents.ClientAccessToken = Guid.NewGuid().ToString("N");
            agents.DeveloperAccessToken = Guid.NewGuid().ToString("N");


            _context.Agents.Add(agents);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgents", new { id = agents.Id }, new { id = agents.Id });
        }

        // DELETE: v1/Agents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgents([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agents = await _context.Agents.SingleOrDefaultAsync(m => m.Id == id);
            if (agents == null)
            {
                return NotFound();
            }

            _context.Agents.Remove(agents);
            await _context.SaveChangesAsync();

            return Ok(agents);
        }

        private bool AgentsExists(string id)
        {
            return _context.Agents.Any(e => e.Id == id);
        }
    }
}