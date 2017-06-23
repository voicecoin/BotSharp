using Eagle.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Controllers
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class RootController : ControllerBase
    {
        protected readonly DataContexts dc;

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
