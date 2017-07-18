using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.DataContexts;
using Eagle.DbTables;
using Eagle.Enums;
using Eagle.Models;

namespace Eagle.DbExtensions
{
    public static partial class DbExtension
    {
        public static TaxonomyTermEntity AddTerm(this TaxonomyEntity taxonomy, CoreDbContext dc, string name, EntityStatus status = EntityStatus.Active)
        {
            TaxonomyTermEntity term = dc.TaxonomyTerms.Add(new TaxonomyTermEntity { CreatedUserId = dc.CurrentUser.Id, Name = name, TaxonomyId = taxonomy.Id, Status = status }).Entity;
            dc.SaveChanges();

            return term;
        }

        public static TaxonomyEntity Add(this IQueryable<TaxonomyEntity> terms, CoreDbContext dc, string bundleId, EntityStatus status = EntityStatus.Active)
        {
            TaxonomyEntity term = dc.Taxonomies.Add(new TaxonomyEntity { CreatedUserId = dc.CurrentUser.Id, BundleId = bundleId, Status = status }).Entity;
            dc.SaveChanges();

            return term;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentTerm"></param>
        /// <param name="dc"></param>
        /// <param name="taxonomyId"></param>
        /// <param name="termTitle"></param>
        /// <returns>parentTerm</returns>
        public static TaxonomyTermEntity AddChildTerm(this TaxonomyTermEntity parentTerm, CoreDbContext dc, string taxonomyId, String termTitle)
        {
            TaxonomyTermEntity childTerm = dc.TaxonomyTerms.Add(new TaxonomyTermEntity { CreatedUserId = dc.CurrentUser.Id, Name = termTitle, TaxonomyId = taxonomyId, ParentId = parentTerm.Id, Status = EntityStatus.Active }).Entity;
            dc.SaveChanges();

            return parentTerm;
        }

        public static TaxonomyTermEntity Add(this IQueryable<TaxonomyTermEntity> terms, CoreDbContext dc, string taxonomyId, string termTitle)
        {
            TaxonomyTermEntity term = dc.TaxonomyTerms.Add(new TaxonomyTermEntity { CreatedUserId = dc.CurrentUser.Id,  Name = termTitle, TaxonomyId = taxonomyId, Status = EntityStatus.Active }).Entity;
            dc.SaveChanges();

            return term;
        }
    }
}
