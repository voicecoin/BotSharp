using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Core.Interfaces;
using Eagle.DataContexts;
using Eagle.DbExtensions;
using Eagle.DbTables;
using Eagle.Enums;

namespace Eagle.Core.Taxonomy
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 1;

        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            LoadDepartment(dc);
            LoadServiceCategory(dc);

            dc.SaveChanges();
        }

        private void LoadDepartment(CoreDbContext dc)
        {
            if (dc.Bundles.Any(x => x.EntityName == "Taxonomy" && x.Name == "Department")) return;

            BundleEntity departmentBundle = dc.Bundles.Add(new BundleEntity {CreatedUserId = dc.CurrentUser.Id,  Name = "Department", EntityName = "Taxonomy", Status = EntityStatus.Active }).Entity;
            TaxonomyEntity departmentTaxonomy = dc.Taxonomies.Add(dc, departmentBundle.Id);

            BundleEntity buBundle = dc.Bundles.Add(new BundleEntity {CreatedUserId = dc.CurrentUser.Id, Name = "Business Unit", EntityName = "Taxonomy", Status = EntityStatus.Active }).Entity;
            TaxonomyEntity buTaxonomy = dc.Taxonomies.Add(dc, buBundle.Id);

            dc.TaxonomyTerms.Add(dc, departmentTaxonomy.Id, "Exterior")
                .AddChildTerm(dc, buTaxonomy.Id, "Landscaping")
                .AddChildTerm(dc, buTaxonomy.Id, "Sweeping")
                .AddChildTerm(dc, buTaxonomy.Id, "Striping");

            dc.TaxonomyTerms.Add(dc, departmentTaxonomy.Id, "Interior")
                .AddChildTerm(dc, buTaxonomy.Id, "Windows")
                .AddChildTerm(dc, buTaxonomy.Id, "Floor Care");

            dc.TaxonomyTerms.Add(dc, departmentTaxonomy.Id, "Facilities")
                .AddChildTerm(dc, buTaxonomy.Id, "Facility");

            dc.TaxonomyTerms.Add(dc, departmentTaxonomy.Id, "SIM")
                .AddChildTerm(dc, buTaxonomy.Id, "SIM");
        }

        private void LoadServiceCategory(CoreDbContext dc)
        {
            if (dc.Bundles.Any(x => x.EntityName == "Taxonomy" && x.Name == "Service Category")) return;

            BundleEntity scategoryBundle = dc.Bundles.Add(new BundleEntity { CreatedUserId = dc.CurrentUser.Id, Name = "Service Category", EntityName = "Taxonomy", Status = EntityStatus.Active }).Entity;
            TaxonomyEntity scategoryTaxonomy = dc.Taxonomies.Add(dc, scategoryBundle.Id);

            BundleEntity stypeBundle = dc.Bundles.Add(new BundleEntity { CreatedUserId = dc.CurrentUser.Id, Name = "Service Type", EntityName = "Taxonomy", Status = EntityStatus.Active }).Entity;
            TaxonomyEntity stypeTaxonomy = dc.Taxonomies.Add(dc, stypeBundle.Id);

            BundleEntity scodeBundle = dc.Bundles.Add(new BundleEntity { CreatedUserId = dc.CurrentUser.Id, Name = "Service Code", EntityName = "Taxonomy", Status = EntityStatus.Active }).Entity;
            TaxonomyEntity scodeTaxonomy = dc.Taxonomies.Add(dc, scodeBundle.Id);

            TaxonomyTermEntity topTermEntity = dc.TaxonomyTerms.Add(dc, scategoryTaxonomy.Id, "Air Conditioning/Heating")
                .AddChildTerm(dc, stypeTaxonomy.Id, "Not cooling")
                .AddChildTerm(dc, stypeTaxonomy.Id, "Not heating")
                .AddChildTerm(dc, stypeTaxonomy.Id, "No airflow");

            TaxonomyTermEntity secondTermEntity = dc.TaxonomyTerms.First(x => x.ParentId == topTermEntity.Id && x.Name.Equals("Not cooling"));
            secondTermEntity.AddChildTerm(dc, scodeTaxonomy.Id, "Sales")
                .AddChildTerm(dc, scodeTaxonomy.Id, "Stockroom")
                .AddChildTerm(dc, scodeTaxonomy.Id, "Office area");

            dc.TaxonomyTerms.First(x => x.ParentId == topTermEntity.Id && x.Name.Equals("Not heating"))
                .AddChildTerm(dc, scodeTaxonomy.Id, "Backroom")
                .AddChildTerm(dc, scodeTaxonomy.Id, "Sales Floor")
                .AddChildTerm(dc, scodeTaxonomy.Id, "Vestibule");
        }
    }
}
