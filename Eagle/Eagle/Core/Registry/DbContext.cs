using Microsoft.EntityFrameworkCore;
using Eagle.DbTables;
using Eagle.Models;

namespace Eagle.DataContexts
{
    public partial class CoreDbContext
    {
        public DbSet<RegistryEntity> Registries { get; set; }
        public DbSet<RegistryEntryEntity> RegistryEntries { get; set; }
    }
}
