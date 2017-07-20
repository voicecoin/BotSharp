using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.DomainModels;

namespace Core.Bundle
{
    public class BundleFieldController : CoreController
    {
        // GET: api/BundleFields
        [HttpGet]
        public IEnumerable<Object> GetBundleFields()
        {
            return dc.Table<BundleEntity>().Select(x => new { Id = x.Id, Name = x.Name, Status = x.Status.ToString(), EntityName = x.EntityName });
        }

        // GET: api/Bundle/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBundleEntity([FromRoute] string id)
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

        // GET: api/BundleField/5/Fields
        [HttpGet("{id}/Fields")]
        public async Task<IActionResult> GetBundleFields([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bundleEntity = from b in dc.Table<BundleEntity>()
                               join bf in dc.Table<BundleFieldEntity>() on b.Id equals bf.BundleId
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
        public async Task<IActionResult> PutBundleFieldEntity([FromRoute] string id, [FromBody] DmBundle bundleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bundleModel.Id)
            {
                return BadRequest();
            }

            bundleModel.Add(dc);

            return NoContent();
        }

        // POST: api/BundleField
        [HttpPost]
        public async Task<IActionResult> PostBundleFieldEntity([FromBody] DmBundleField bundleFieldModel)
        {
            bundleFieldModel.Add(dc);

            return CreatedAtAction("PostBundleFieldEntity", new { id = bundleFieldModel.Id }, bundleFieldModel);
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