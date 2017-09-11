using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core;
using Core.Field;
using Microsoft.EntityFrameworkCore;
using Models;
using Core.Bundle;
using Core.Interfaces;
using Core.Enums;
using Newtonsoft.Json.Linq;

namespace Core.Node
{
    [Table("Nodes")]
    public class NodeEntity : BundleDbRecord, IBundlable, IDbRecord4Core
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
    public class NodeBooleanFieldEntity : BooleanFieldEntity, IDbRecord4Core { }
    [Table("NodeTextFields")]
    public class NodeTextFieldEntity : TextFieldEntity, IDbRecord4Core { }
    [Table("NodeRichTextFields")]
    public class NodeRichTextFieldEntity : RichTextFieldEntity, IDbRecord4Core { }
    [Table("NodeAddressFields")]
    public class NodeAddressFieldEntity : AddressFieldEntity, IDbRecord4Core { }
    [Table("NodeEntityReferenceFields")]
    public class NodeEntityReferenceFieldEntity : EntityReferenceFieldEntity, IDbRecord4Core { }
    [Table("NodeTaxonomyTermFields")]
    public class NodeTaxonomyTermFieldEntity : TaxonomyTermFieldEntity, IDbRecord4Core { }
    [Table("NodeNumberFields")]
    public class NodeNumberFieldEntity : NumberFieldEntity, IDbRecord4Core { }
    [Table("NodeCurrencyFields")]
    public class NodeCurrencyFieldEntity : CurrencyFieldEntity, IDbRecord4Core { }
    [Table("NodeDateTimeFields")]
    public class NodeDateTimeFieldEntity : DateTimeFieldEntity, IDbRecord4Core { }
    [Table("NodeImageFields")]
    public class NodeImageFieldEntity : ImageFieldEntity, IDbRecord4Core { }
}
