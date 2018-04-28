using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using EntityFrameworkCore.BootKit;
using DotNetToolkit;
using DotNetToolkit.JwtHelper;
using Voicebot.Core.Account;

namespace Voicebot.RestApi.Account
{
    public class AccountController : CoreController
    {
        [AllowAnonymous]
        [HttpPost("token")]
        [ProducesResponseType(typeof(String), 200)]
        public IActionResult Token([FromBody] VmUserLogin userModel)
        {
            if (String.IsNullOrEmpty(userModel.Email) || String.IsNullOrEmpty(userModel.Password))
            {
                return new BadRequestObjectResult("Username and password should not be empty.");
            }

            // validate from local
            var user = (from usr in dc.Table<User>()
                        join auth in dc.Table<UserAuth>() on usr.Id equals auth.UserId
                        where usr.Email == userModel.Email
                        select auth).FirstOrDefault();

            if (user != null)
            {
                if (!user.IsActivated)
                {
                    return BadRequest("Account hasn't been activated, please check your email to activate it.");
                }
                else
                {
                    // validate password
                    string hash = PasswordHelper.Hash(userModel.Password, user.Salt);
                    if (user.Password == hash)
                    {
                        return Ok(JwtToken.GenerateToken(Database.Configuration, user.UserId));
                    }
                    else
                    {
                        return BadRequest("Authorization Failed.");
                    }
                }
            }
            else
            {
                return BadRequest("Account doesn't exist");
            }
        }

        private IActionResult Ok(object p)
        {
            throw new NotImplementedException();
        }
    }
}
