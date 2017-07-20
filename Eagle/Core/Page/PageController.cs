using Core.Block;
using Core.Interfaces;
using Core.Node;
using Microsoft.AspNetCore.Authorization;
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
            var page = new DmPage {
                Id = Guid.NewGuid().ToString(),
                Name = "Users",
                Description = "List all authorized users."
            };

            page.Blocks.Add(new DmBlock
            {
                DataUrl = "/api/View/1/Execute",
                Name = "Users List",
                Priority = 1,
                Position = new DmBlockPosition { Width = 12, Height = 5, X = 0, Y = 0 },
                Menus = new List<KeyValuePair<String, String>>() {
                    new KeyValuePair<string, string>("Menu1", "/"),
                    new KeyValuePair<string, string>("Menu1", "/")
                }
            });

            return Ok(page);
        }

        [HttpPost]
        public async Task<IActionResult> PostPage(DmPage pageModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dc.Transaction<IDbRecord4SqlServer>(delegate
            {
                pageModel.Add(dc);
            });
            
            return CreatedAtAction("GetPage", new { id = pageModel.Id }, pageModel);
        }
    }
}
