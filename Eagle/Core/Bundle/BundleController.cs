using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Core.Bundle
{
    public class BundleController : CoreController
    {
        // GET: api/Bundle
        [HttpGet]
        public IEnumerable<Object> GetBundles()
        {
            return dc.Table<BundleEntity>().Where(x => !x.EntityName.Equals("Taxonomy")).Select(x => new { Id = x.Id, Name = x.Name, Status = x.Status.ToString(), EntityName = x.EntityName });
        }

        // GET: api/Bundle/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBundleEntity([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bundleEntity = await dc.Table<BundleEntity>().Include(x => x.Fields).SingleOrDefaultAsync(m => m.Id == id);

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
            
            dc.Table<BundleEntity>().Add(bundleEntity);
            dc.SaveChanges();

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

            var bundleEntity = await dc.Table<BundleEntity>().SingleOrDefaultAsync(m => m.Id == id);
            if (bundleEntity == null)
            {
                return NotFound();
            }

            return Ok(bundleEntity);
        }

        private bool BundleEntityExists(string id)
        {
            return dc.Table<BundleEntity>().Any(e => e.Id == id);
        }
    }
}