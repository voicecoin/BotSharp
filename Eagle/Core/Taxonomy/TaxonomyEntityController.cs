using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Core.Taxonomy
{
    public class TaxonomyEntityController : CoreController
    {
        // GET: api/TaxonomyEntity/5
        [HttpGet("{id}")]
        public IEnumerable<Object> GetTaxonomyEntities([FromRoute] string id)
        {
            var taxonomies = from b in dc.Bundles
                        join t in dc.Taxonomies on b.Id equals t.BundleId
                        where b.Id == id
                        select new { Name = b.Name, EntityName = b.EntityName, BundleName = b.Name, Status = t.Status.ToString() };

            return taxonomies;
        }
    }
}
