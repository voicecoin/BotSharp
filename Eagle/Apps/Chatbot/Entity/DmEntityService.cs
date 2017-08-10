using Apps.Chatbot.DomainModels;
using Apps.Chatbot.Entity;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Apps.Chatbot.DmServices
{
    public static class DmEntityService
    {
        public static void Add(this DmEntity entityModel, CoreDbContext dc)
        {
            if (String.IsNullOrEmpty(entityModel.Id))
            {
                entityModel.Id = Guid.NewGuid().ToString();
            }
            
            entityModel.Color = LinqExtensions.GetRandomColor();
            var entityRecord = entityModel.Map<EntityEntity>();
            entityRecord.CreatedDate = DateTime.UtcNow;
            entityRecord.CreatedUserId = dc.CurrentUser.Id;
            entityRecord.ModifiedDate = DateTime.UtcNow;
            entityRecord.ModifiedUserId = dc.CurrentUser.Id;

            dc.Table<EntityEntity>().Add(entityRecord);

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

        public static void Delete(this DmEntity entityModel, CoreDbContext dc)
        {
            EntityEntity entityRecored = dc.Table<EntityEntity>().Find(entityModel.Id);

            // remove entries
            entityModel.DeleteEntries(dc, entityRecored.Id);
            dc.Table<EntityEntity>().Remove(entityRecored);
        }

        public static void DeleteEntries(this DmEntity entityModel, CoreDbContext dc, string entityId)
        {
            if (entityModel.Entries == null) return;

            // remove entries
            entityModel.Entries.ToList().ForEach(entry =>
            {
                entry.Delete(dc);
            });
        }

        public static void Update(this DmEntity entityModel, CoreDbContext dc)
        {
            EntityEntity entityRecored = dc.Table<EntityEntity>().Find(entityModel.Id);
            entityRecored.Name = entityModel.Name;
            entityRecored.Description = entityModel.Description;
            entityRecored.IsEnum = entityModel.IsEnum;
        }

        public static void Add(this DmEntityEntry entityEntryModel, CoreDbContext dc)
        {
            entityEntryModel.Id = Guid.NewGuid().ToString();
            var entryRecord = entityEntryModel.Map<EntityEntryEntity>();
            entryRecord.CreatedDate = DateTime.UtcNow;
            entryRecord.CreatedUserId = dc.CurrentUser.Id;
            entryRecord.ModifiedDate = DateTime.UtcNow;
            entryRecord.ModifiedUserId = dc.CurrentUser.Id;

            dc.Table<EntityEntryEntity>().Add(entryRecord);
            // add synonyms
            entityEntryModel.AddSynonyms(dc, entryRecord.Id);
        }

        public static void Delete(this DmEntityEntry entityEntryModel, CoreDbContext dc)
        {
            EntityEntryEntity entityEntryRecored = dc.Table<EntityEntryEntity>().Find(entityEntryModel.Id);

            entityEntryModel.DeleteSynonyms(dc, entityEntryModel.Id);
            dc.Table<EntityEntryEntity>().Remove(entityEntryRecored);
        }

        public static void Update(this DmEntityEntry entityEntryModel, CoreDbContext dc)
        {
            EntityEntryEntity entityEntryRecored = dc.Table<EntityEntryEntity>().Find(entityEntryModel.Id);
            entityEntryRecored.Value = entityEntryModel.Value;

            entityEntryModel.DeleteSynonyms(dc, entityEntryRecored.Id);
            entityEntryModel.AddSynonyms(dc, entityEntryModel.Id);
        }

        public static void AddSynonyms(this DmEntityEntry entityEntryModel, CoreDbContext dc, string entityEntryId)
        {
            if (entityEntryModel.Synonyms == null) entityEntryModel.Synonyms = new List<String>();

            entityEntryModel.Synonyms.Where(x => !String.IsNullOrEmpty(x)).ToList().ForEach(synonym =>
            {
                dc.Table<EntityEntrySynonymEntity>().Add(new EntityEntrySynonymEntity
                {
                    EntityEntryId = entityEntryId,
                    Synonym = synonym,
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = dc.CurrentUser.Id,
                    ModifiedDate = DateTime.UtcNow,
                    ModifiedUserId = dc.CurrentUser.Id
                });

            });
        }

        public static void DeleteSynonyms(this DmEntityEntry entityEntryModel, CoreDbContext dc, string entityEntryId)
        {
            if (entityEntryModel.Synonyms == null) return;

            dc.Table<EntityEntrySynonymEntity>().RemoveRange(dc.Table<EntityEntrySynonymEntity>().Where(x => x.EntityEntryId == entityEntryId));
        }
    }
}
