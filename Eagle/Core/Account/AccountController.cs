using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Core.DbTables;

namespace Core.Account
{
    public class AccountController : CoreController
    {
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var user = dc.Users.Find(GetCurrentUser().Id);

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{userName}/Exist")]
        public async Task<IActionResult> UserExist([FromRoute] String userName)
        {
            var user = dc.Users.Any(x => x.UserName == userName);

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] String id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(dc.Users.Find(id));
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
            accountModel.BundleId = dc.Bundles.First(x => x.Name == "User Profile").Id;
            accountModel.Id = Guid.NewGuid().ToString();
            accountModel.CreatedUserId = accountModel.Id;
            accountModel.CreatedDate = DateTime.UtcNow;
            accountModel.ModifiedUserId = accountModel.Id;
            accountModel.ModifiedDate = DateTime.UtcNow;

            dc.Users.Add(accountModel);
            await dc.SaveChangesAsync();

            return CreatedAtAction("CreateUser", new { id = accountModel.Id }, accountModel.Id);
        }
    }
}