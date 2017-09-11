using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces;
using Core.Node;

namespace Core.Account
{
    public class AccountController : CoreController
    {
        [HttpGet("users")]
        public DmPageResult<UserEntity> GetUsers(string name, [FromQuery] int page = 1)
        {
            var query = dc.Table<UserEntity>().AsQueryable();
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.UserName.Contains(name));
            }

            var total = query.Count();
            int size = 20;
            var items = query.Skip((page - 1) * size).Take(size).ToList();

            return new DmPageResult<UserEntity> { Total = total, Page = page, Size = size, Items = items };
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var user = dc.Table<UserEntity>().Find(GetCurrentUser().Id);

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Exist")]
        public async Task<IActionResult> UserExist([FromQuery] String userName)
        {
            var user = dc.Table<UserEntity>().Any(x => x.UserName == userName);

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] String id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(dc.Table<UserEntity>().Find(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] String id, UserEntity accountModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction("UpdateUser", new { id = accountModel.Id }, accountModel.Id);
        }

        // POST: api/Account
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserEntity accountModel)
        {
            dc.Transaction<IDbRecord4Core>(delegate {
                var dm = new BundleDomainModel<UserEntity>(dc, accountModel);
                dm.AddEntity();
            });

            return CreatedAtAction("CreateUser", new { id = accountModel.Id }, accountModel.Id);
        }
    }
}