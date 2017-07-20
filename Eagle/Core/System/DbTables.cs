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

            Triggers<DbRecord>.Updated += entry =>
            {

            };

            Triggers<DbRecord>.UpdateFailed += entry =>
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
            throw new NotImplementedException();
        }

        public virtual string GetEntityName()
        {
            string className = this.GetType().Name;
            return className.Substring(0, className.Length - 6);
        }
    }

    public abstract class BundleDbRecord : DbRecord
    {
        [Required]
        public string BundleId { get; set; }
    }
}
