using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.Models;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Model.Extionsions
{
    public static class ModelExtension
    {
        public static void Create(this EntityModel entityModel, DataContexts dc)
        {
            entityModel.Id = Guid.NewGuid().ToString();
            var entityRecord = entityModel.Map<Entities>();

            dc.Entities.Add(entityRecord);


            // add entries
            if (entityModel.Entries != null)
            {
                entityModel.Entries.Where(x => !String.IsNullOrEmpty(x.Value)).ToList().ForEach(entry =>
                {
                    entry.EntityId = entityRecord.Id;
                    entry.Create(dc);
                });
            }
        }

        public static void Delete(this EntityModel entityModel, DataContexts dc)
        {
            Entities entityRecored = dc.Entities.Find(entityModel.Id);

            // remove entries
            if (entityModel.Entries != null)
            {
                entityModel.Entries.ToList().ForEach(entry =>
                {
                    dc.EntityEntrySynonyms.RemoveRange(dc.EntityEntrySynonyms.Where(x => x.EntityEntryId == entry.Id));
                });

                dc.EntityEntries.RemoveRange(dc.EntityEntries.Where(x => x.EntityId == entityRecored.Id));
            }

            dc.Entities.Remove(entityRecored);
        }

        public static void Update(this EntityModel entityModel, DataContexts dc)
        {
            Entities entityRecored = dc.Entities.Find(entityModel.Id);
            entityRecored.Name = entityModel.Name;
        }

        public static void Create(this EntityEntryModel entityModel, DataContexts dc)
        {
            var entryRecord = entityModel.Map<EntityEntries>();

            dc.EntityEntries.Add(entryRecord);

            // add synonyms
            if (entityModel.Synonyms != null)
            {
                entityModel.Synonyms.Where(x => !String.IsNullOrEmpty(x)).ToList().ForEach(synonym =>
                    {

                        dc.EntityEntrySynonyms.Add(new EntityEntrySynonyms
                        {
                            EntityEntryId = entryRecord.Id,
                            Synonym = synonym
                        });

                    });
            }
        }
    }
}
