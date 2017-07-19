using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DbTables;
using Models;

namespace Core.DataContexts
{
    public partial class CoreDbContext
    {
        public DbSet<NodeEntity> Nodes { get; set; }
        public DbSet<NodeTextFieldEntity> NodeTextFields { get; set; }
        public DbSet<NodeRichTextFieldEntity> NodeRichTextFields { get; set; }
        public DbSet<NodeAddressFieldEntity> NodeAddressFields { get; set; }
        public DbSet<NodeEntityReferenceFieldEntity> NodeEntityReferenceFields { get; set; }
        public DbSet<NodeTaxonomyTermFieldEntity> NodeTaxonomyTermFields { get; set; }
        public DbSet<NodeNumberFieldEntity> NodeNumberFields { get; set; }
        public DbSet<NodeCurrencyFieldEntity> NodeCurrencyFields { get; set; }
        public DbSet<NodeDateTimeFieldEntity> NodeDateTimeFields { get; set; }
        public DbSet<NodeImageFieldEntity> NodeImageFields { get; set; }
    }
}
