using Core.Bundle;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        public void ValideModel(ModelStateDictionary modelState)
        {
            modelState.Remove("Id");
            modelState.Remove("BundleId");
            modelState.Remove("CreatedUserId");
            modelState.Remove("CreatedDate");
            modelState.Remove("ModifiedUserId");
            modelState.Remove("ModifiedDate");
        }

        /// <summary>
        /// Core DbRecord
        /// </summary>
        public T Entity { get; set; }

        public String BundleId { get; set; }
        public void LoadFieldRecords()
        {
            throw new NotImplementedException();
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
