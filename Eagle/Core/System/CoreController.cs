using Core.Account;
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
        protected readonly CoreDbContext dc;
        
        protected CoreDbContext Dc { get; set; }

        public CoreController()
        {
            //dc = new CoreDbContext(new DbContextOptions<CoreDbContext>() { });

            dc = new CoreDbContext();
            dc.CurrentUser = GetCurrentUser();
            dc.InitDb();
            Dc = dc;
        }

        protected String GetConfig(string path)
        {
            return CoreDbContext.Configuration.GetSection(path).Value;
        }

        protected UserEntity GetCurrentUser()
        {
            if (this.User != null)
            {
                return new UserEntity
                {
                    Id = this.User.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value,
                    UserName = this.User.Claims.FirstOrDefault(x => x.Type.Equals("UserName"))?.Value
                };
            }
            else
            {
                return new UserEntity
                {
                    Id = Guid.Empty.ToString(),
                    UserName = "Anonymous"
                };
            }
        }
    }
}
