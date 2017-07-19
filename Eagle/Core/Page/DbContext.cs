using Core.Page;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DataContexts
{
    public partial class CoreDbContext
    {
        public DbSet<PageEntity> Pages { get; set; }
    }
}
