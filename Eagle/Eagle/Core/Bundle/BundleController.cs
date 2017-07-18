using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eagle.Models;
using Eagle.DbTables;
using Eagle.DbTables;

namespace Eagle.Core.Bundle
{
    public class BundleController : CoreController
    {
        // GET: api/Bundle
        [HttpGet]
        public IEnumerable<Object> GetBundles()
        {
            return dc.Bundles.Where(x => !x.EntityName.Equals("Taxonomy")).Select(x => new { Id = x.Id, Name = x.Name, Status = x.Status.ToString(), EntityName = x.EntityName });
        }

        // GET: api/Bundle/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBundleEntity([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bundleEntity = await dc.Bundles.Include(x => x.Fields).SingleOrDefaultAsync(m => m.Id == id);

            if (bundleEntity == null)
            {
                return NotFound();
            }

            return Ok(bundleEntity);
        }

        // PUT: api/Bundle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBundleEntity([FromRoute] string id, [FromBody] BundleEntity bundleEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bundleEntity.Id)
            {
                return BadRequest();
            }

            dc.Entry(bundleEntity).State = EntityState.Modified;

            try
            {
                await dc.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BundleEntityExists(id))
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

        // POST: api/Bundle
        [HttpPost]
        public async Task<IActionResult> PostBundleEntity([FromBody] BundleEntity bundleEntity)
        {
            bundleEntity.CreatedUserId = GetCurrentUser().Id;
            bundleEntity.CreatedDate = DateTime.UtcNow;
            bundleEntity.ModifiedUserId = GetCurrentUser().Id;
            bundleEntity.ModifiedDate = DateTime.UtcNow;
            
            dc.Bundles.Add(bundleEntity);
            await dc.SaveChangesAsync();

            return CreatedAtAction("GetBundleEntity", new { id = bundleEntity.Id }, bundleEntity);
        }

        // DELETE: api/Bundle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBundleEntity([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bundleEntity = await dc.Bundles.SingleOrDefaultAsync(m => m.Id == id);
            if (bundleEntity == null)
            {
                return NotFound();
            }

            dc.Bundles.Remove(bundleEntity);
            await dc.SaveChangesAsync();

            return Ok(bundleEntity);
        }

        private bool BundleEntityExists(string id)
        {
            return dc.Bundles.Any(e => e.Id == id);
        }
    }
}