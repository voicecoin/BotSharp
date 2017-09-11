using Core.Block;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Core.Page
{
    [Table("Pages")]
    public class PageEntity : DbRecord, IDbRecord4Core
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Entity Name cannot be longer than 50 characters.")]
        public String Name { get; set; }
        [MaxLength(128)]
        public String Description { get; set; }
        [MaxLength(64)]
        public String UrlPath { get; set; }

        [NotMapped]
        public List<BlockEntity> Blocks { get; set; }

        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<PageEntity>().Any(x => x.Name == Name);
        }
    }

    [Table("PageBlocks")]
    public class PageBlockEntity : DbRecord, IDbRecord4Core
    {
        [Required]
        [StringLength(36)]
        public string PageId { get; set; }
        [Required]
        [StringLength(36)]
        public string BlockId { get; set; }
        [MaxLength(128)]
        public String PositionJson { get; set; }
        [NotMapped]
        public DmBlockPositionInPage Position { get; set; }
        public override bool IsExist(CoreDbContext dc)
        {
            return dc.Table<PageBlockEntity>().Any(x => x.PageId == PageId && x.BlockId == BlockId);
        }
    }

    public class DmBlockPositionInPage
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
