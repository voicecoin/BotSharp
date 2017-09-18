using EntityFrameworkCore.Triggers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Core.Enums;
using Core.Interfaces;
using Core.Node;
using Core.Bundle;

namespace Core
{
    /// <summary>
    /// All other model must derive from EntityModel
    /// https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell
    /// https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/new-db
    /// We are using TPH model, https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/inheritance
    /// Add-Migration Initial -context "VDbContext"
    /// Update-Database -context "VDbContext"
    /// </summary>
    public abstract class DbRecord : IDbRecord
    {
        static DbRecord()
        {
            Triggers<DbRecord>.Inserting += entry =>
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
                entry.Entity.ModifiedDate = entry.Entity.CreatedDate;
            };

            Triggers<DbRecord>.Inserted += entry =>
            {
            };

            Triggers<DbRecord>.Deleting += entry =>
            {

            };

            Triggers<DbRecord>.Deleted += entry =>
            {

            };

            Triggers<DbRecord>.Updating += entry =>
            {
                entry.Entity.ModifiedDate = DateTime.UtcNow;
            };

            Triggers<IDbRecord>.Updated += entry =>
            {

            };

            Triggers<IDbRecord>.UpdateFailed += entry =>
            {

            };
        }

        [Key]
        [StringLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String Id { get; set; }

        public EntityStatus Status { get; set; }

        /// <summary>
        /// https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/concurrency
        /// </summary>
        [Timestamp]
        [JsonIgnore]
        public byte[] RowVersion { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
#if AUTH_REQUIRED
        [Required]
#endif
        [StringLength(36)]
        public string CreatedUserId { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
#if AUTH_REQUIRED
        [Required]
#endif
        [StringLength(36)]
        public string ModifiedUserId { get; set; }

        public virtual bool IsExist(CoreDbContext dc)
        {
            return false;
        }

        public virtual bool InitRecord(CoreDbContext dc)
        {
            return true;
        }
    }

    public abstract class BundleDbRecord : DbRecord
    {
        [Required]
        public string BundleId { get; set; }
        public string GetEntityName<T>()
        {
            Type type = typeof(T);
            return type.Name.Substring(0, type.Name.Length - 6);
        }

        [NotMapped]
        public List<DmFieldRecord> FieldRecords { get; set; }
        public IQueryable<NodeTextFieldEntity> LoadTextFields(CoreDbContext dc)
        {
            var query = from bundleField in dc.Table<BundleFieldEntity>()
                        join textField in dc.Table<NodeTextFieldEntity>() on bundleField.Id equals textField.BundleFieldId
                        where bundleField.BundleId == BundleId
                            && bundleField.FieldTypeId.Equals(FieldTypes.Text)
                            && textField.EntityId == Id
                        select textField;

            string sql = query.ToString();

            return query;
        }

        public void LoadFieldRecords(CoreDbContext dc)
        {
            var FieldRecords = new List<object>();
            dc.Table<BundleFieldEntity>().Where(x => x.BundleId == BundleId).ToList().ForEach(field =>
            {
                switch (field.FieldTypeId)
                {
                    case FieldTypes.Text:
                        FieldRecords.AddRange(dc.Table<NodeTextFieldEntity>().Where(x => x.BundleFieldId == field.Id));
                        break;
                }
            });
        }
    }
}
