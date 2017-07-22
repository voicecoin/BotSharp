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

namespace Core.Node
{
    [Table("Nodes")]
    public class NodeEntity : DbRecord, IDbRecord4SqlServer
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        [Required]
        public string BundleId { get; set; }
        public String Description { get; set; }
    }

    [Table("NodeTextFields")]
    public class NodeTextFieldEntity : TextFieldEntity, IDbRecord4SqlServer { }
    [Table("NodeRichTextFields")]
    public class NodeRichTextFieldEntity : RichTextFieldEntity, IDbRecord4SqlServer { }
    [Table("NodeAddressFields")]
    public class NodeAddressFieldEntity : AddressFieldEntity, IDbRecord4SqlServer { }
    [Table("NodeEntityReferenceFields")]
    public class NodeEntityReferenceFieldEntity : EntityReferenceFieldEntity, IDbRecord4SqlServer { }
    [Table("NodeTaxonomyTermFields")]
    public class NodeTaxonomyTermFieldEntity : TaxonomyTermFieldEntity, IDbRecord4SqlServer { }
    [Table("NodeNumberFields")]
    public class NodeNumberFieldEntity : NumberFieldEntity, IDbRecord4SqlServer { }
    [Table("NodeCurrencyFields")]
    public class NodeCurrencyFieldEntity : CurrencyFieldEntity, IDbRecord4SqlServer { }
    [Table("NodeDateTimeFields")]
    public class NodeDateTimeFieldEntity : DateTimeFieldEntity, IDbRecord4SqlServer { }
    [Table("NodeImageFields")]
    public class NodeImageFieldEntity : ImageFieldEntity, IDbRecord4SqlServer { }
}
