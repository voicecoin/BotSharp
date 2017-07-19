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
        public DbSet<TaxonomyEntity> Taxonomies { get; set; }
        public DbSet<TaxonomyTermEntity> TaxonomyTerms { get; set; }
    }
}
