using Core.Block;
using Core.Interfaces;
using Core.Node;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Block
{
    public class BlockController : CoreController
    {
        [HttpGet("Query")]
        public DmPageResult<BlockEntity> GetBlocks(string name, [FromQuery] int page = 1)
        {
            var query = dc.Table<BlockEntity>().AsQueryable();
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            return new DmPageResult<BlockEntity>() { Page = page }.LoadDataByPage(query);
        }

        [HttpPost]
        public async Task<IActionResult> PostBlock(BlockEntity blockEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dc.Transaction<IDbRecord4Core>(delegate
            {
                var dm = new DomainModel<BlockEntity>(dc, blockEntity);
                dm.AddEntity();
            });
            
            return CreatedAtAction("GetBlock", new { id = blockEntity.Id }, blockEntity.Id);
        }
    }
}
