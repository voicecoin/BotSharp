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

namespace Core.Page
{
    public class PageController : CoreController
    {
        [HttpGet("Query")]
        public DmPageResult<PageEntity> GetPages(string name, [FromQuery] int page = 1)
        {
            var query = dc.Table<PageEntity>().AsQueryable();
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            return new DmPageResult<PageEntity>() { Page = page }.LoadDataByPage(query);
        }

        /*[AllowAnonymous]
        [HttpGet("Paths")]
        public IEnumerable<Object> GetPagePaths(string name, [FromQuery] int page = 1, [FromServices] IMemcachedClient memcachedClient = null)
        {
            dc.MemcachedClient = memcachedClient;

            var query = dc.Table<PageEntity>().AsQueryable();
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            return new DmPageResult<PageEntity>() { Page = page }.LoadDataByPage(query).Items.Select(x => new { PageId = x.Id, Path = x.UrlPath });
        }*/

        // GET: api/Page/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPage([FromRoute] string id)
        {
            var dm = new DomainModel<PageEntity>(dc, new PageEntity { Id = id });
            dm.Load();

            return Ok(dm.Entity);
        }

        [HttpPost]
        public async Task<IActionResult> PostPage(PageEntity pageEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dc.Transaction<IDbRecord4Core>(delegate
            {
                var dm = new DomainModel<PageEntity>(dc, pageEntity);
                DmPageService.Add(dm);
            });
            
            return CreatedAtAction("GetPage", new { id = pageEntity.Id }, pageEntity.Id);
        }
    }
}
