using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Utility;

namespace Core
{
    [Route("http")]
    public class HttpController : CoreController
    {
        [HttpPost]
        public async Task<object> Http([FromBody] JObject data, [FromQuery] string host, [FromQuery] string path)
        {
            string hostUrl = GetConfig(host);
            var user = GetCurrentUser();
            data.Add("CurrentUserName", user.UserName);
            return await HttpClientHelper.PostAsJsonAsync<Object>(hostUrl, path, data);
        }
    }
}