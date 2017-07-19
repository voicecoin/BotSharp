using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core;
using Core.Field;
using Microsoft.EntityFrameworkCore;
using Models;
using Core.DbTables;
using Core.DataContexts;
using Core.Bundle;

namespace Core.DbTables
{
    public class NodeEntity : DbRecordWithName
    {
        [Required]
        public string BundleId { get; set; }
        public String Description { get; set; }
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
