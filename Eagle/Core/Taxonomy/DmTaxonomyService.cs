using Core.Bundle;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Core.Taxonomy
{
    public static class DmTaxonomyService
    {
        public static void Add(this TaxonomyEntity taxonomyModel, CoreDbContext dc)
        {
            if (dc.Table<BundleEntity>().Any(x => x.Name == taxonomyModel.Description && x.EntityName == "Taxonomy")) return;

            var dbRecord = taxonomyModel.Map<BundleEntity>();

            dbRecord.Status = EntityStatus.Active;
            dbRecord.CreatedUserId = dc.CurrentUser.Id;
            dbRecord.CreatedDate = DateTime.UtcNow;
            dbRecord.ModifiedUserId = dc.CurrentUser.Id;
            dbRecord.ModifiedDate = DateTime.UtcNow;

            dc.Table<BundleEntity>().Add(dbRecord);
        }

        public static TaxonomyTermEntity AddTerm(this TaxonomyEntity taxonomy, CoreDbContext dc, string name, EntityStatus status = EntityStatus.Active)
        {
            TaxonomyTermEntity entity = new TaxonomyTermEntity
            {
                CreatedUserId = dc.CurrentUser.Id,
                CreatedDate = DateTime.UtcNow,
                ModifiedUserId = dc.CurrentUser.Id,
                ModifiedDate = DateTime.UtcNow,
                Name = name,
                TaxonomyId = taxonomy.Id,
                Status = status
            };
            TaxonomyTermEntity term = dc.Table<TaxonomyTermEntity>().Add(entity).Entity;
            dc.SaveChanges();

            return term;
        }

        public static TaxonomyEntity Add(this IQueryable<TaxonomyEntity> terms, CoreDbContext dc, string bundleId, EntityStatus status = EntityStatus.Active)
        {
            TaxonomyEntity entity = new TaxonomyEntity
            {
                CreatedUserId = dc.CurrentUser.Id,
                CreatedDate = DateTime.UtcNow,
                ModifiedUserId = dc.CurrentUser.Id,
                ModifiedDate = DateTime.UtcNow,
                BundleId = bundleId,
                Status = status
            };

            TaxonomyEntity term = dc.Table<TaxonomyEntity>().Add(entity).Entity;
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
            TaxonomyTermEntity entity = new TaxonomyTermEntity
            {
                CreatedUserId = dc.CurrentUser.Id,
                CreatedDate = DateTime.UtcNow,
                ModifiedUserId = dc.CurrentUser.Id,
                ModifiedDate = DateTime.UtcNow,
                Name = termTitle,
                TaxonomyId = taxonomyId,
                ParentId = parentTerm.Id,
                Status = EntityStatus.Active
            };

            TaxonomyTermEntity childTerm = dc.Table<TaxonomyTermEntity>().Add(entity).Entity;
            dc.SaveChanges();

            return parentTerm;
        }

        public static TaxonomyTermEntity Add(this IQueryable<TaxonomyTermEntity> terms, CoreDbContext dc, string taxonomyId, string termTitle)
        {
            TaxonomyTermEntity entity = new TaxonomyTermEntity
            {
                CreatedUserId = dc.CurrentUser.Id,
                CreatedDate = DateTime.UtcNow,
                ModifiedUserId = dc.CurrentUser.Id,
                ModifiedDate = DateTime.UtcNow,
                Name = termTitle,
                TaxonomyId = taxonomyId,
                Status = EntityStatus.Active
            };

            TaxonomyTermEntity term = dc.Table<TaxonomyTermEntity>().Add(entity).Entity;
            dc.SaveChanges();

            return term;
        }
    }
}
