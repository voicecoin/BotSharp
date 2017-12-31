using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core.Field;
using Core.Bundle;
using Core.Enums;
using Newtonsoft.Json.Linq;
using EntityFrameworkCore.BootKit;

namespace Core.Node
{
    [Table("Nodes")]
    public class NodeEntity : BundleDbRecord, IBundlable, IDbRecord
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        public String Description { get; set; }
    }

    public class DmFieldRecord
    {
        public FieldTypes FieldTypeId { get; set; }
        public string BundleFieldId { get; set; }
        public List<JObject> Data { get; set; }
    }

    [Table("NodeBooleanFields")]
    public class NodeBooleanFieldEntity : BooleanFieldEntity, IDbRecord { }
    [Table("NodeTextFields")]
    public class NodeTextFieldEntity : TextFieldEntity, IDbRecord { }
    [Table("NodeRichTextFields")]
    public class NodeRichTextFieldEntity : RichTextFieldEntity, IDbRecord { }
    [Table("NodeAddressFields")]
    public class NodeAddressFieldEntity : AddressFieldEntity, IDbRecord { }
    [Table("NodeEntityReferenceFields")]
    public class NodeEntityReferenceFieldEntity : EntityReferenceFieldEntity, IDbRecord { }
    [Table("NodeTaxonomyTermFields")]
    public class NodeTaxonomyTermFieldEntity : TaxonomyTermFieldEntity, IDbRecord { }
    [Table("NodeNumberFields")]
    public class NodeNumberFieldEntity : NumberFieldEntity, IDbRecord { }
    [Table("NodeCurrencyFields")]
    public class NodeCurrencyFieldEntity : CurrencyFieldEntity, IDbRecord { }
    [Table("NodeDateTimeFields")]
    public class NodeDateTimeFieldEntity : DateTimeFieldEntity, IDbRecord { }
    [Table("NodeImageFields")]
    public class NodeImageFieldEntity : ImageFieldEntity, IDbRecord { }
}
