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
        public async Task<IActionResult> GetUsers()
        {
            var users = dc.Table<UserEntity>();

            return Ok(users);
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
            var exist = dc.Table<UserEntity>().Any(x => x.UserName == userName);

            return Ok(exist);
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
        public async Task<IActionResult> CreateUser(UserEntity accountModel)
        {
            dc.Transaction<IDbRecord4SqlServer>(delegate {
                accountModel.FirstName = accountModel.UserName.Split('@')[0];
                var dm = new BundleDomainModel<UserEntity>(dc, accountModel);
                dm.AddEntity();
            });

            return Ok(new { UserName = accountModel.UserName });
        }
    }
}