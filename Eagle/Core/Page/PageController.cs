using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Page
{
    public class PageController : CoreController
    {
        // GET: api/Page/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPage([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var node = await dc.Nodes.SingleOrDefaultAsync(m => m.Id == id);

            if (node == null)
            {
                return NotFound();
            }

            return Ok(node);
        }

        [HttpPost]
        public async Task<IActionResult> PostPage(DmPage pageModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dc.Transaction(delegate
            {
                pageModel.Add(dc);
            });
            
            return CreatedAtAction("GetPage", new { id = pageModel.Id }, pageModel);
        }
    }
}
