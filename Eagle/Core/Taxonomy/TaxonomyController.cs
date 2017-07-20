using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Core.Bundle;

namespace Core.Taxonomy
{
    public class TaxonomyController : CoreController
    {
        // GET: api/Taxonomy
        [HttpGet]
        public IEnumerable<Object> GetTaxonomies()
        {
            
            var query = from b in dc.Table<BundleEntity>()
                        join t in dc.Table<TaxonomyEntity>() on b.Id equals t.BundleId
                        select new
                        {
                            Id = t.Id,
                            Name = b.Name,
                            Description = t.Description,
                            EntityName = b.EntityName,
                            BundleId = b.Id,
                            Status = t.Status.ToString()
                        };

            return query.ToList();
        }

        // GET: api/Taxonomy/Terms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaxonomy([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taxonomy = await dc.Table<TaxonomyEntity>().SingleOrDefaultAsync(m => m.Id == id);

            if (taxonomy == null)
            {
                return NotFound();
            }

            return Ok(taxonomy);
        }

        [HttpGet("{id}/Terms")]
        public IEnumerable<Object> GetTaxonomyTerms([FromRoute] string id)
        {
            var query = from t in dc.Table<TaxonomyEntity>()
                        join bundle in dc.Table<BundleEntity>() on t.BundleId equals bundle.Id
                        join tt in dc.Table<TaxonomyTermEntity>() on t.Id equals tt.TaxonomyId
                        from ttp in dc.Table<TaxonomyTermEntity>().Where(x => tt.ParentId == x.Id).DefaultIfEmpty()
                        where bundle.Id == id
                        select new { Id = t.Id, Name = tt.Name, Status = tt.Status.ToString(), Parent = ttp == null ? null : ttp.Name, ParentId = tt.ParentId };

            return query.ToList();
        }

        // PUT: api/Taxonomy/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaxonomy([FromRoute] string id, [FromBody] TaxonomyEntity taxonomy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taxonomy.Id)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Taxonomy
        [HttpPost]
        public async Task<IActionResult> PostTaxonomy(TaxonomyEntity taxonomy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction("GetTaxonomy", new { id = taxonomy.Id }, taxonomy);
        }

        // DELETE: api/Taxonomy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaxonomy([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(null);
        }

        private bool TaxonomyExists(string id)
        {
            return dc.Table<TaxonomyEntity>().Any(e => e.Id == id);
        }
    }
}