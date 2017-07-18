using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Eagle.Core;
using Eagle.Core.Field;
using Eagle.DataContexts;
using Microsoft.EntityFrameworkCore;
using Eagle.Models;

namespace Eagle.DbTables
{
    public class NodeEntity : DbRecordWithNameColumn, IBundle
    {
        [Required]
        public string BundleId { get; set; }
        public String Description { get; set; }

        public IQueryable<NodeTextFieldEntity> LoadTextFields(CoreDbContext dc)
        {
            var query = from bundleField in dc.BundleFields
                        join textField in dc.NodeTextFields on bundleField.Id equals textField.BundleFieldId
                        where bundleField.BundleId == BundleId
                            && bundleField.FieldTypeId.Equals(Enums.FieldTypes.Text)
                            && textField.EntityId == Id
                        select textField;

            string sql = query.ToString();

            return query;
        }

        [NotMapped]
        public List<Object> FieldRecords { get; set; }
        public void LoadFieldRecords(CoreDbContext dc)
        {
            FieldRecords = new List<object>();
            dc.BundleFields.Where(x => x.BundleId == BundleId).ToList().ForEach(field =>
            {
                switch (field.FieldTypeId)
                {
                    case Enums.FieldTypes.Text:
                        FieldRecords.AddRange(dc.NodeTextFields.Where(x => x.BundleFieldId == field.Id));
                        break;
                }
            });
        }
    }

    public class NodeTextFieldEntity : TextFieldEntity { }
    public class NodeRichTextFieldEntity : RichTextFieldEntity { }
    public class NodeAddressFieldEntity : AddressFieldEntity { }
    public class NodeEntityReferenceFieldEntity : EntityReferenceFieldEntity { }
    public class NodeTaxonomyTermFieldEntity : TaxonomyTermFieldEntity { }
    public class NodeNumberFieldEntity : NumberFieldEntity { }
    public class NodeCurrencyFieldEntity : CurrencyFieldEntity { }
    public class NodeDateTimeFieldEntity : DateTimeFieldEntity { }
    public class NodeImageFieldEntity : ImageFieldEntity { }
}
