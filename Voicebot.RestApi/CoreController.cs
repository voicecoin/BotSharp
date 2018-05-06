using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voicebot.RestApi
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

        protected String CurrentUserId
        {
            get
            {
                return this.User.Claims.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value;
            }
        }
    }
}
