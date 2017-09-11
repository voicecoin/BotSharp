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
        public static void Add(this EntityEntity entityModel, CoreDbContext dc)
        {
            if (String.IsNullOrEmpty(entityModel.Id))
            {
                entityModel.Id = Guid.NewGuid().ToString();
            }
            
            entityModel.Color = LinqExtensions.GetRandomColor();
            entityModel.CreatedDate = DateTime.UtcNow;
            entityModel.CreatedUserId = dc.CurrentUser.Id;
            entityModel.ModifiedDate = DateTime.UtcNow;
            entityModel.ModifiedUserId = dc.CurrentUser.Id;

            dc.Table<EntityEntity>().Add(entityModel);

            // add entries
            if (entityModel.Entries != null)
            {
                entityModel.Entries.Where(x => !String.IsNullOrEmpty(x.Value)).ToList().ForEach(entry =>
                {
                    entry.EntityId = entityModel.Id;
                    entry.Add(dc);
                });
            }
        }

        public static void Delete(this EntityEntity entityModel, CoreDbContext dc)
        {
            EntityEntity entityRecored = dc.Table<EntityEntity>().Find(entityModel.Id);

            dc.Table<EntityEntryEntity>().RemoveRange(dc.Table<EntityEntryEntity>().Where(x => x.EntityId == entityModel.Id));
            dc.Table<EntityEntrySynonymEntity>().RemoveRange(dc.Table<EntityEntrySynonymEntity>().Where(x => x.EntityEntryId == entityModel.Id));

            dc.Table<EntityEntity>().Remove(entityRecored);
        }

        public static void Update(this EntityEntity entityModel, CoreDbContext dc)
        {
            EntityEntity entityRecored = dc.Table<EntityEntity>().Find(entityModel.Id);
            entityRecored.Name = entityModel.Name;
            entityRecored.Description = entityModel.Description;
            entityRecored.IsEnum = entityModel.IsEnum;
        }

        public static void Add(this EntityEntryEntity entityEntryModel, CoreDbContext dc)
        {
            entityEntryModel.Id = Guid.NewGuid().ToString();
            entityEntryModel.CreatedDate = DateTime.UtcNow;
            entityEntryModel.CreatedUserId = dc.CurrentUser.Id;
            entityEntryModel.ModifiedDate = DateTime.UtcNow;
            entityEntryModel.ModifiedUserId = dc.CurrentUser.Id;

            dc.Table<EntityEntryEntity>().Add(entityEntryModel);
            // add synonyms
            entityEntryModel.AddSynonyms(dc, entityEntryModel.Id);
        }

        public static void Delete(this EntityEntryEntity entityEntryModel, CoreDbContext dc)
        {
            EntityEntryEntity entityEntryRecored = dc.Table<EntityEntryEntity>().Find(entityEntryModel.Id);

            entityEntryModel.DeleteSynonyms(dc, entityEntryModel.Id);
            dc.Table<EntityEntryEntity>().Remove(entityEntryRecored);
        }

        public static void Update(this EntityEntryEntity entityEntryModel, CoreDbContext dc)
        {
            EntityEntryEntity entityEntryRecored = dc.Table<EntityEntryEntity>().Find(entityEntryModel.Id);
            entityEntryRecored.Value = entityEntryModel.Value;

            entityEntryModel.DeleteSynonyms(dc, entityEntryRecored.Id);
            entityEntryModel.AddSynonyms(dc, entityEntryModel.Id);
        }

        public static void AddSynonyms(this EntityEntryEntity entityEntryModel, CoreDbContext dc, string entityEntryId)
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

        public static void DeleteSynonyms(this EntityEntryEntity entityEntryModel, CoreDbContext dc, string entityEntryId)
        {
            if (entityEntryModel.Synonyms == null) return;

            dc.Table<EntityEntrySynonymEntity>().RemoveRange(dc.Table<EntityEntrySynonymEntity>().Where(x => x.EntityEntryId == entityEntryId));
        }
    }
}
