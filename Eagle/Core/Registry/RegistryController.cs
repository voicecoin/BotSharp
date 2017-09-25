using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Registry
{
    public class RegistryController : CoreController
    {
        [AllowAnonymous]
        [HttpGet("SiteSettings")]
        public IActionResult GetSettings()
        {
            IEnumerable<IConfigurationSection> settings = CoreDbContext.Configuration.GetSection("SiteSetting").GetChildren();

            JObject result = new JObject();
            settings.ToList().ForEach(setting => {
                result.Add(setting.Key, setting.Value);
            });

            return  Ok(result);
        }
    }
}
