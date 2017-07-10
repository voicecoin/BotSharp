using Eagle.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Core
{
    //[Produces("application/json", "text/plain")]
    [Route("v1/[controller]")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    public class CoreController : ControllerBase
    {
        public static IConfigurationRoot Configuration { get; set; }
        protected readonly DataContexts dc;

        public CoreController()
        {
            dc = new DataContexts(new DbContextOptions<DataContexts>() { });
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
