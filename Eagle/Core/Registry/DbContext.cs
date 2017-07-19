using Microsoft.EntityFrameworkCore;
using Core.DbTables;
using Models;

namespace Core.DataContexts
{
    public partial class CoreDbContext
    {
        public DbSet<RegistryEntity> Registries { get; set; }
        public DbSet<RegistryEntryEntity> RegistryEntries { get; set; }
    }
}
