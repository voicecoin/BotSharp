using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.DataContexts;
using Eagle.DbTables;
using Eagle.Enums;
using Eagle.Models;

namespace Eagle.DbExtensions
{
    public static partial class DbExtension
    {
        public static String Value(this RegistryEntity registry, CoreDbContext dc, string entryName)
        {
            String value = (from r in dc.Registries
                                        join re in dc.RegistryEntries on r.Id equals re.RegistryId
                                        where re.Name == entryName
                                        select re.Value).FirstOrDefault();
                                        
            return value;
        }

        public static RegistryEntryEntity AddEntry(this RegistryEntity registry, CoreDbContext dc, string entryName, string entryValue)
        {
            RegistryEntryEntity entry = dc.RegistryEntries.Add(new RegistryEntryEntity { Name = entryName, Value = entryValue, Status = EntityStatus.Freezing }).Entity;

            return entry;
        }
    }
}
