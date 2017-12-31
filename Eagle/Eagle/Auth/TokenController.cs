using Core;
using Core.Account;
using Core.Interfaces;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Eagle.Auth
{
    [Route("token")]
    [AllowAnonymous]
    public class TokenController : CoreController
    {
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                return new BadRequestObjectResult("Username and password should not be empty.");
            }

            // Auth through SMS ONE
            var Configuration = CoreDbContext.Configuration;
            string redirect = Configuration.GetSection("SiteSetting:DefaultPage").Value;

            // validate from local
            var user = dc.Table<UserEntity>().FirstOrDefault(x => x.UserName == username);
            if(user != null)
            {
                return Ok(new
                {
                    Token = GenerateToken(username, user.Id),
                    Redirect = redirect,
                    UserId = user.Id
                });
            }
            
            return new BadRequestObjectResult($"Authorization Failed");
        }

        private string GenerateToken(string userName, string userId)
        {
            var Configuration = CoreDbContext.Configuration;
            var authConfig = Configuration.GetSection("TokenAuthentication");

            var token = new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(authConfig["SecretKey"]))
                .AddSubject("james bond")
                .AddIssuer(authConfig["Issuer"])
                .AddAudience(authConfig["Audience"])
                .AddClaim("UserName", userName)
                .AddClaim("UserId", userId)
                .AddExpiry(120)
                .Build();

            return token.Value;
        }
    }
}
