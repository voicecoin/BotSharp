using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Enums;
using Core.Registry;
using Core;

namespace DbExtensions
{
    public static partial class DbExtension
    {
        public static String Value(this RegistryEntity registry, CoreDbContext dc, string entryName)
        {
            String value = (from r in dc.Table<RegistryEntity>()
                                        join re in dc.Table<RegistryEntryEntity>() on r.Id equals re.RegistryId
                                        where re.Name == entryName
                                        select re.Value).FirstOrDefault();
                                        
            return value;
        }

        public static RegistryEntryEntity AddEntry(this RegistryEntity registry, CoreDbContext dc, string entryName, string entryValue)
        {
            RegistryEntryEntity entry = dc.Table<RegistryEntryEntity>().Add(new RegistryEntryEntity { Name = entryName, Value = entryValue, Status = EntityStatus.Freezing }).Entity;

            return entry;
        }
    }
}
