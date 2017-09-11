using Core.Bundle;
using Core.Enums;
using Core.Interfaces;
using Core.Node;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class DomainModel<T> : IDomainModel<T> where T : DbRecord
    {
        public CoreDbContext Dc { get; set; }

        public DomainModel(CoreDbContext db, T dbRecord)
        {
            Dc = db;
            Entity = dbRecord;

            if (String.IsNullOrEmpty(Entity.CreatedUserId))
            {
                Entity.CreatedUserId = Dc.CurrentUser.Id;
                Entity.CreatedDate = DateTime.UtcNow;
            }

            if (String.IsNullOrEmpty(Entity.ModifiedUserId))
            {
                Entity.ModifiedUserId = Dc.CurrentUser.Id;
                Entity.ModifiedDate = DateTime.UtcNow;
            }

            if (String.IsNullOrEmpty(Entity.Id))
            {
                Entity.Id = Guid.NewGuid().ToString();

                Entity.InitRecord(db);
            }
        }
        /// <summary>
        /// Core DbRecord
        /// </summary>
        public T Entity;

        public void ValideModel(ModelStateDictionary modelState)
        {
            modelState.Remove("Id");
            modelState.Remove("CreatedUserId");
            modelState.Remove("CreatedDate");
            modelState.Remove("ModifiedUserId");
            modelState.Remove("ModifiedDate");
        }

        public bool AddEntity()
        {
            if (Entity.IsExist(Dc)) return false;

            Dc.Table<T>().Add(Entity);
            Dc.SaveChanges();

            return true;
        }

        public bool RemoveEntity()
        {
            throw new NotImplementedException();
        }

        public T LoadEntity()
        {
            Entity = Dc.Table<T>().Find(Entity.Id);
            return Entity;
        }
    }

    public class BundleDomainModel<T> : IDomainModel<T>, IBundlable<T> where T : BundleDbRecord
    {
        public CoreDbContext Dc;

        public BundleDomainModel(CoreDbContext db, T dbRecord)
        {
            Dc = db;
            Entity = dbRecord;

            if (String.IsNullOrEmpty(Entity.BundleId))
            {
                string entityName = Entity.GetEntityName<T>();
                var bundle = Dc.Table<BundleEntity>().First(x => x.EntityName == entityName);

                if (bundle == null) throw new Exception("Bundle not found");

                Entity.BundleId = bundle.Id;
            }

            if (String.IsNullOrEmpty(Entity.CreatedUserId))
            {
                Entity.CreatedUserId = Dc.CurrentUser.Id;
                Entity.CreatedDate = DateTime.UtcNow;
            }

            if (String.IsNullOrEmpty(Entity.ModifiedUserId))
            {
                Entity.ModifiedUserId = Dc.CurrentUser.Id;
                Entity.ModifiedDate = DateTime.UtcNow;
            }

            if (String.IsNullOrEmpty(Entity.Id))
            {
                Entity.Id = Guid.NewGuid().ToString();

                Entity.InitRecord(db);
            }
        }

        /// <summary>
        /// Core DbRecord
        /// </summary>
        public T Entity { get; set; }

        public String BundleId { get; set; }

        public void LoadFieldRecords()
        {
            var dc = Dc;

            var FieldRecords = new List<DmFieldRecord>();
            dc.Table<BundleFieldEntity>().Where(x => x.BundleId == BundleId).ToList().ForEach(field =>
            {
                switch (field.FieldTypeId)
                {
                    case FieldTypes.Text:
                        var textFields = dc.Table<NodeTextFieldEntity>().Where(x => x.EntityId == Entity.Id && x.BundleFieldId == field.Id).ToList();
                        FieldRecords.Add(new DmFieldRecord
                        {
                             BundleFieldId = BundleId,
                             FieldTypeId = FieldTypes.Text,
                             Data = textFields.Select(x => JObject.FromObject(x)).ToList()
                        });
                        break;
                    case FieldTypes.Boolean:
                        var boolFields = dc.Table<NodeBooleanFieldEntity>().Where(x => x.EntityId == Entity.Id && x.BundleFieldId == field.Id).ToList();
                        FieldRecords.Add(new DmFieldRecord
                        {
                            BundleFieldId = BundleId,
                            FieldTypeId = FieldTypes.Boolean,
                            Data = boolFields.Select(x => JObject.FromObject(x)).ToList()
                        });
                        break;
                    case FieldTypes.Address:
                        var addressFields = dc.Table<NodeAddressFieldEntity>().Where(x => x.EntityId == Entity.Id && x.BundleFieldId == field.Id).ToList();
                        FieldRecords.Add(new DmFieldRecord
                        {
                            BundleFieldId = BundleId,
                            FieldTypeId = FieldTypes.Address,
                            Data = addressFields.Select(x => JObject.FromObject(x)).ToList()
                        });
                        break;
                }
            });

            Entity.FieldRecords = FieldRecords;
        }

        public void ValideModel(ModelStateDictionary modelState)
        {
            modelState.Remove("Id");
            modelState.Remove("BundleId");
            modelState.Remove("CreatedUserId");
            modelState.Remove("CreatedDate");
            modelState.Remove("ModifiedUserId");
            modelState.Remove("ModifiedDate");
        }

        public bool AddEntity()
        {
            if (Entity.IsExist(Dc)) return false;

            Dc.Table<T>().Add(Entity);
            Dc.SaveChanges();

            return true;
        }

        public bool RemoveEntity()
        {
            throw new NotImplementedException();
        }

        public T LoadEntity()
        {
            Entity = Dc.Table<T>().Find(Entity.Id);
            BundleId = Entity.BundleId;

            LoadFieldRecords();

            return Entity;
        }
    }


    public class DmQuery<T>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public T Data { get; set; }
    }

    public class DmPageResult<T>
    {
        public DmPageResult()
        {
            Page = 1;
            Size = 20;
        }

        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
        public List<T> Items { get; set; }
    }

    public class Constants
    {
        public static readonly string SystemUserId = "28d2bf0c-c84f-4b63-b0d7-f1a459a09ade";
    }

}
