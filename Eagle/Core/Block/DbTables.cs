using Core.Page;
using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Core.Block
{
    [Table("Blocks")]
    public class BlockEntity : CoreDbRecord, IDbRecord
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        public String Description { get; set; }
        public String ViewId { get; set; }

        public int Priority { get; set; }

        [NotMapped]
        public List<KeyValuePair<String, String>> Menus { get; set; }
        [NotMapped]
        public DmBlockPositionInPage Position { get; set; }
        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<BlockEntity>().Any(x => x.Name == Name);
        }
    }
}
