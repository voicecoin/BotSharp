using Core.Bundle;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        }
        /// <summary>
        /// Core DbRecord
        /// </summary>
        public T Entity;

        public bool AddEntity()
        {
            if (Entity.IsExist(Dc)) return false;

            if (String.IsNullOrEmpty(Entity.Id))
            {
                Entity.Id = Guid.NewGuid().ToString();
            }

            Entity.CreatedUserId = Dc.CurrentUser.Id;
            Entity.CreatedDate = DateTime.UtcNow;
            Entity.ModifiedUserId = Dc.CurrentUser.Id;
            Entity.ModifiedDate = DateTime.UtcNow;

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

            string entityName = Entity.GetEntityName<T>();
            var bundle = Dc.Table<BundleEntity>().First(x => x.EntityName == entityName);

            if (bundle == null) return false;

            if (String.IsNullOrEmpty(Entity.Id))
            {
                Entity.Id = Guid.NewGuid().ToString();
            }

            Entity.BundleId = bundle.Id;
            Entity.CreatedUserId = Dc.CurrentUser.Id;
            Entity.CreatedDate = DateTime.UtcNow;
            Entity.ModifiedUserId = Dc.CurrentUser.Id;
            Entity.ModifiedDate = DateTime.UtcNow;

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
