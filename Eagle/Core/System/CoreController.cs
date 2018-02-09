using ContentFoundation.Core.Account;
using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Core
{
    [Authorize]
    [Produces("application/json")]
    [Route("v1/[controller]")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    public class CoreController : ControllerBase
    {
        protected readonly Database dc;
        
        protected Database Dc { get; set; }

        public CoreController()
        {
            dc = new DefaultDataContextLoader().GetDefaultDc();
        }

        protected String GetConfig(string path)
        {
            return Database.Configuration.GetSection(path).Value;
        }

        protected User GetCurrentUser()
        {
            if (this.User != null)
            {
                return new User
                {
                    Id = this.User.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value,
                    Name = this.User.Claims.FirstOrDefault(x => x.Type.Equals("UserName"))?.Value
                };
            }
            else
            {
                return new User
                {
                    Id = Guid.Empty.ToString(),
                    Name = "Anonymous"
                };
            }
        }
    }
}
