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
        private CoreDbContext dc;
        public DomainModel(CoreDbContext db, T dbRecord)
        {
            dc = db;
            entity = dbRecord;
        }
        /// <summary>
        /// Core DbRecord
        /// </summary>
        private T entity;

        public bool Add()
        {
            if (entity.IsExist(dc)) return false;

            if (String.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
            }

            entity.CreatedUserId = dc.CurrentUser.Id;
            entity.CreatedDate = DateTime.UtcNow;
            entity.ModifiedUserId = dc.CurrentUser.Id;
            entity.ModifiedDate = DateTime.UtcNow;

            dc.Table<T>().Add(entity);

            return true;
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }

    public class BundleDomainModel<T> : IDomainModel<T>, IBundlable<T> where T: BundleDbRecord
    {
        private CoreDbContext dc;
        public BundleDomainModel(CoreDbContext db, T dbRecord)
        {
            dc = db;
            entity = dbRecord;
        }
        /// <summary>
        /// Core DbRecord
        /// </summary>
        private T entity;

        public String BundleId { get; set; }
        public void LoadFieldRecords()
        {
            throw new NotImplementedException();
        }

        public bool Add()
        {
            if (entity.IsExist(dc)) return false;

            string entityName = entity.GetEntityName();
            var bundle = dc.Table<BundleEntity>().First(x => x.EntityName == entityName);

            if (bundle == null) return false;

            if (String.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
            }

            entity.BundleId = bundle.Id;
            entity.CreatedUserId = dc.CurrentUser.Id;
            entity.CreatedDate = DateTime.UtcNow;
            entity.ModifiedUserId = dc.CurrentUser.Id;
            entity.ModifiedDate = DateTime.UtcNow;

            dc.Table<T>().Add(entity);

            return true;
        }

        public void Remove()
        {
            throw new NotImplementedException();
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
