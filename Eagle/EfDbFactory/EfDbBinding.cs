using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace EfDbFactory
{
    public class EfDbBinding
    {
        public DbConnection connection4Master { get; set; }
        internal EfDbContext DbContextMaster { get; set; }
        public List<DbConnection> connection4Slaves { get; set; }
        internal List<EfDbContext> DbContextSlavers { get; set; }
        public Type EntityBaseType { get; set; }
        public Type DbContextType { get; set; }
        public List<Type> EntityTypeList { get; set; }
    }
}
