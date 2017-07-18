using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Core;
using Eagle.DbTables;
using Eagle.DataContexts;

namespace Eagle.DbTables
{
    public class TaxonomyEntity : DbRecord, IBundle
    {
        [Required]
        public string BundleId { get; set; }
        [NotMapped]
        public List<Object> FieldRecords { get; set; }
        public void LoadFieldRecords(CoreDbContext dc)
        {
            FieldRecords = new List<object>();

            List<BundleFieldEntity> fields = dc.BundleFields.Where(x => x.BundleId == BundleId).ToList();
            foreach (BundleFieldEntity field in fields)
            {
                /*if (field.FieldTypeName.Equals("Address"))
                {
                    var records = dc.LocationAddressRecords.Where(x => x.EntityId == Id && x.BundleId == field.BundleId);
                    FieldRecords.AddRange(records);
                }
                else if (field.FieldTypeName.Equals("Email"))
                {
                    var records = dc.LocationEmailRecords.Where(x => x.EntityId == Id && x.BundleId == field.BundleId);
                    FieldRecords.AddRange(records);
                }
                else if (field.FieldTypeName.Equals("Phone"))
                {
                    var records = dc.LocationPhoneRecords.Where(x => x.EntityId == Id && x.BundleId == field.BundleId);
                    FieldRecords.AddRange(records);
                }*/
            }
        }

        public string GetEntityTypeName()
        {
            throw new NotImplementedException();
        }

        [MaxLength(512, ErrorMessage = "Description cannot be longer than 512 characters.")]
        public String Description { get; set; }
    }

    public class TaxonomyTermEntity : DbRecordWithNameColumn
    {
        [Required]
        public string TaxonomyId { get; set; }
        public TaxonomyEntity Taxonomy { get; set; }

        /// <summary>
        /// Parent Term, In order to create taxonomy with hierarchy
        /// </summary>
        public string ParentId { get; set; }
    }
}
