using Core.Block;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Core.Page
{
    public static class DmPageService
    {
        public static void Add(this DomainModel<PageEntity> pageModel)
        {
            pageModel.AddEntity();
            pageModel.Entity.Blocks.ForEach(block => {
                var dm = new DomainModel<PageBlockEntity>(pageModel.Dc, new PageBlockEntity { BlockId = block.Id, PageId = pageModel.Entity.Id });
                dm.Entity.PositionJson = JsonConvert.SerializeObject(new DmBlockPositionInPage { Width = 12, Height = 5, X = 0, Y = 0 });
                dm.AddEntity();
            });
        }

        public static void Load(this DomainModel<PageEntity> pageModel)
        {
            CoreDbContext dc = pageModel.Dc;
            pageModel.LoadEntity();
            var blocks = (from b in dc.Table<BlockEntity>()
                                      join pb in dc.Table<PageBlockEntity>() on b.Id equals pb.BlockId
                                      where pb.PageId == pageModel.Entity.Id
                                      select new { Id = pb.Id, Block = b, PositionJson = pb.PositionJson }).ToList();

            pageModel.Entity.Blocks = blocks.Select(x => {
                x.Block.Position = JsonConvert.DeserializeObject<DmBlockPositionInPage>(x.PositionJson);
                return x.Block;
            }).ToList();
        }
    }
}
