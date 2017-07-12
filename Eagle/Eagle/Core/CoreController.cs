using Eagle.DataContexts;
using Eagle.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Core
{
#if AUTH_REQUIRED
    [Authorize]
#endif
    //[Produces("application/json", "application/xml")]
    [Route("v1/[controller]")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    public class CoreController : ControllerBase
    {
        public static IConfigurationRoot Configuration { get; set; }
        protected readonly CoreDbContext dc;

        public CoreController()
        {
            dc = new CoreDbContext(new DbContextOptions<CoreDbContext>() { });
        }

        protected DmAccount GetCurrentUser()
        {
            if (this.User != null)
            {
                return new DmAccount
                {
                    Id = this.User.Claims.First(x => x.Type.Equals("UserId")).Value,
                    UserName = this.User.Identity.Name
                };
            }
            else
            {
                return new DmAccount
                {
                    Id = Guid.Empty.ToString(),
                    UserName = "Anonymous"
                };
            }
        }

        /*[HttpGet]
        public object RestApiTest()
        {
            var client = new RestClient("http://localhost:9200/");
            var request = new RestRequest();

            IRestResponse response = client.Execute(request);

            return response.Content;
        }*/
    }
}
