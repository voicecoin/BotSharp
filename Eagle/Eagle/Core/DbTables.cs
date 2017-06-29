using Eagle.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.DbTables
{
    public abstract class DbTable
    {
        public DbTable()
        {
            Id = Guid.NewGuid().ToString();
            DataStatus = DataRowStatus.Active;
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow;
        }

        [Key]
        [StringLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String Id { get; set; }

        // 用户信息通过ClientAccessToken或者DeveloperAccessToken信息获取
        /*[Required]
        [StringLength(36)]
        public String CreatedUserId { get; set; }*/
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
        [Required]
        public DataRowStatus DataStatus { get; set; }
    }
}