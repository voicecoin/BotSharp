using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eagle.Enums;
using Eagle.DbTables;

namespace Eagle.Core.Bundle
{
    public class BundleFieldController : CoreController
    {
        // GET: api/BundleFields
        [HttpGet]
        public IEnumerable<Object> GetBundleFields()
        {
            return dc.Bundles.Select(x => new { Id = x.Id, Name = x.Name, Status = x.Status.ToString(), EntityName = x.EntityName });
        }

        // GET: api/Bundle/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBundleEntity([FromRoute] string id)
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

            return Ok(bundleEntity);
        }

        // GET: api/BundleField/5/Fields
        [HttpGet("{id}/Fields")]
        public async Task<IActionResult> GetBundleFields([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bundleEntity = from b in dc.Bundles
                               join bf in dc.BundleFields on b.Id equals bf.BundleId
                               where b.Id == id
                               select new { EntityName = b.EntityName, BundleName = b.Name, FieldName = bf.Name, FieldTypeId = bf.FieldTypeId, FieldTypeName = bf.FieldTypeId.ToString(), Status = bf.Status.ToString() };

            if (bundleEntity == null)
            {
                return NotFound();
            }

            return Ok(bundleEntity);
        }

        // PUT: api/Bundle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBundleFieldEntity([FromRoute] string id, [FromBody] BundleEntity bundleEntity)
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

        // POST: api/BundleField
        [HttpPost]
        public async Task<IActionResult> PostBundleFieldEntity([FromBody] BundleFieldEntity bundleFieldEntity)
        {
            bundleFieldEntity.Status = EntityStatus.Active;
            bundleFieldEntity.CreatedUserId = GetCurrentUser().Id;
            bundleFieldEntity.CreatedDate = DateTime.UtcNow;
            bundleFieldEntity.ModifiedUserId = GetCurrentUser().Id;
            bundleFieldEntity.ModifiedDate = DateTime.UtcNow;

            dc.BundleFields.Add(bundleFieldEntity);
            await dc.SaveChangesAsync();

            return CreatedAtAction("PostBundleFieldEntity", new { id = bundleFieldEntity.Id }, bundleFieldEntity);
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