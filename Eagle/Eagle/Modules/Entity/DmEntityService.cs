using Eagle.DbContexts;
using Eagle.DbTables;
using Eagle.DomainModels;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DmServices
{
    public static class DmEntityService
    {
        public static void Add(this DmEntity entityModel, DataContexts dc)
        {
            entityModel.Id = Guid.NewGuid().ToString();
            var entityRecord = entityModel.Map<Entities>();
            entityRecord.CreatedDate = DateTime.UtcNow;

            dc.Entities.Add(entityRecord);

            // add entries
            if (entityModel.Entries != null)
            {
                entityModel.Entries.Where(x => !String.IsNullOrEmpty(x.Value)).ToList().ForEach(entry =>
                {
                    entry.EntityId = entityRecord.Id;
                    entry.Add(dc);
                });
            }
        }

        public static void Delete(this DmEntity entityModel, DataContexts dc)
        {
            Entities entityRecored = dc.Entities.Find(entityModel.Id);

            // remove entries
            entityModel.DeleteEntries(dc, entityRecored.Id);
            dc.Entities.Remove(entityRecored);
        }

        public static void DeleteEntries(this DmEntity entityModel, DataContexts dc, string entityId)
        {
            if (entityModel.Entries == null) return;

            // remove entries
            entityModel.Entries.ToList().ForEach(entry =>
            {
                entry.Delete(dc);
            });
        }

        public static void Update(this DmEntity entityModel, DataContexts dc)
        {
            Entities entityRecored = dc.Entities.Find(entityModel.Id);
            entityRecored.Name = entityModel.Name;
            entityRecored.IsEnum = entityModel.IsEnum;
        }

        public static void Add(this DmEntityEntry entityEntryModel, DataContexts dc)
        {
            entityEntryModel.Id = Guid.NewGuid().ToString();
            var entryRecord = entityEntryModel.Map<EntityEntries>();
            entryRecord.CreatedDate = DateTime.UtcNow;

            dc.EntityEntries.Add(entryRecord);
            // add synonyms
            entityEntryModel.AddSynonyms(dc, entryRecord.Id);
        }

        public static void Delete(this DmEntityEntry entityEntryModel, DataContexts dc)
        {
            EntityEntries entityEntryRecored = dc.EntityEntries.Find(entityEntryModel.Id);

            entityEntryModel.DeleteSynonyms(dc, entityEntryModel.Id);
            dc.EntityEntries.Remove(entityEntryRecored);
        }

        public static void Update(this DmEntityEntry entityEntryModel, DataContexts dc)
        {
            EntityEntries entityEntryRecored = dc.EntityEntries.Find(entityEntryModel.Id);
            entityEntryRecored.Value = entityEntryModel.Value;

            entityEntryModel.DeleteSynonyms(dc, entityEntryRecored.Id);
            entityEntryModel.AddSynonyms(dc, entityEntryModel.Id);
        }

        public static void AddSynonyms(this DmEntityEntry entityEntryModel, DataContexts dc, string entityEntryId)
        {
            if (entityEntryModel.Synonyms == null) return;

            entityEntryModel.Synonyms.Where(x => !String.IsNullOrEmpty(x)).ToList().ForEach(synonym =>
            {
                dc.EntityEntrySynonyms.Add(new EntityEntrySynonyms
                {
                    EntityEntryId = entityEntryId,
                    Synonym = synonym,
                    CreatedDate = DateTime.UtcNow
                });

            });
        }

        public static void DeleteSynonyms(this DmEntityEntry entityEntryModel, DataContexts dc, string entityEntryId)
        {
            if (entityEntryModel.Synonyms == null) return;

            dc.EntityEntrySynonyms.RemoveRange(dc.EntityEntrySynonyms.Where(x => x.EntityEntryId == entityEntryId));
        }
    }
}
