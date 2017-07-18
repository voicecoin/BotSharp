using Eagle.Apps.Chatbot.DomainModels;
using Eagle.DataContexts;
using Eagle.DbTables;
using Eagle.DomainModels;
using Eagle.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Apps.Chatbot.DmServices
{
    public static class DmEntityService
    {
        public static void Add(this DmEntity entityModel, CoreDbContext dc)
        {
            if (String.IsNullOrEmpty(entityModel.Id))
            {
                entityModel.Id = Guid.NewGuid().ToString();
            }
            
            entityModel.Color = ObjectExtensions.GetRandomColor();
            var entityRecord = entityModel.Map<Entities>();
            entityRecord.CreatedDate = DateTime.UtcNow;

            dc.Chatbot_Entities.Add(entityRecord);

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
            Entities entityRecored = dc.Chatbot_Entities.Find(entityModel.Id);

            // remove entries
            entityModel.DeleteEntries(dc, entityRecored.Id);
            dc.Chatbot_Entities.Remove(entityRecored);
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
            Entities entityRecored = dc.Chatbot_Entities.Find(entityModel.Id);
            entityRecored.Name = entityModel.Name;
            entityRecored.IsEnum = entityModel.IsEnum;
        }

        public static void Add(this DmEntityEntry entityEntryModel, CoreDbContext dc)
        {
            entityEntryModel.Id = Guid.NewGuid().ToString();
            var entryRecord = entityEntryModel.Map<EntityEntries>();
            entryRecord.CreatedDate = DateTime.UtcNow;

            dc.Chatbot_EntityEntries.Add(entryRecord);
            // add synonyms
            entityEntryModel.AddSynonyms(dc, entryRecord.Id);
        }

        public static void Delete(this DmEntityEntry entityEntryModel, CoreDbContext dc)
        {
            EntityEntries entityEntryRecored = dc.Chatbot_EntityEntries.Find(entityEntryModel.Id);

            entityEntryModel.DeleteSynonyms(dc, entityEntryModel.Id);
            dc.Chatbot_EntityEntries.Remove(entityEntryRecored);
        }

        public static void Update(this DmEntityEntry entityEntryModel, CoreDbContext dc)
        {
            EntityEntries entityEntryRecored = dc.Chatbot_EntityEntries.Find(entityEntryModel.Id);
            entityEntryRecored.Value = entityEntryModel.Value;

            entityEntryModel.DeleteSynonyms(dc, entityEntryRecored.Id);
            entityEntryModel.AddSynonyms(dc, entityEntryModel.Id);
        }

        public static void AddSynonyms(this DmEntityEntry entityEntryModel, CoreDbContext dc, string entityEntryId)
        {
            if (entityEntryModel.Synonyms == null) return;

            entityEntryModel.Synonyms.Where(x => !String.IsNullOrEmpty(x)).ToList().ForEach(synonym =>
            {
                dc.Chatbot_EntityEntrySynonyms.Add(new EntityEntrySynonyms
                {
                    EntityEntryId = entityEntryId,
                    Synonym = synonym,
                    CreatedDate = DateTime.UtcNow
                });

            });
        }

        public static void DeleteSynonyms(this DmEntityEntry entityEntryModel, CoreDbContext dc, string entityEntryId)
        {
            if (entityEntryModel.Synonyms == null) return;

            dc.Chatbot_EntityEntrySynonyms.RemoveRange(dc.Chatbot_EntityEntrySynonyms.Where(x => x.EntityEntryId == entityEntryId));
        }
    }
}
