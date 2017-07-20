using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Core.Bundle;

namespace Core.Taxonomy
{
    public class TaxonomyEntityController : CoreController
    {
        // GET: api/TaxonomyEntity/5
        [HttpGet("{id}")]
        public IEnumerable<Object> GetTaxonomyEntities([FromRoute] string id)
        {
            var taxonomies = from b in dc.Table<BundleEntity>()
                        join t in dc.Table<TaxonomyEntity>() on b.Id equals t.BundleId
                        where b.Id == id
                        select new { Name = b.Name, EntityName = b.EntityName, BundleName = b.Name, Status = t.Status.ToString() };

            return taxonomies;
        }
    }
}
