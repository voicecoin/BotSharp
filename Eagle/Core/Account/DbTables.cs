using Core.Bundle;
using Core.Entity;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Account
{
    [Table("Users")]
    public class UserEntity : BundleDbRecord, IDbRecord4SqlServer
    {
        [Required]
        [StringLength(32)]
        public String UserName { get; set; }
        [Required]
        [StringLength(128)]
        public String Password { get; set; }
        [StringLength(64)]
        public String Email { get; set; }
        [StringLength(32)]
        public String FirstName { get; set; }
        [StringLength(50)]
        public String MiddleName { get; set; }
        [StringLength(32)]
        public String LastName { get; set; }
        /// <summary>
        /// 用户头像 image/base64
        /// </summary>
        [StringLength(4096)]
        public String Avatar { get; set; }
        [MaxLength(256)]
        public String Description { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<UserEntity>().Any(x => x.UserName == UserName);
        }
    }
}
