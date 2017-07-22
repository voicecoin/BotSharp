using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Registry
{
    public class RegistryController : CoreController
    {
        [AllowAnonymous]
        [HttpGet("SiteSettings")]
        public IActionResult GetSettings()
        {
            return Ok(new
            {
                Title = Configuration.GetSection("SiteSetting:Title").Value,
                Slogan = Configuration.GetSection("SiteSetting:Slogan").Value,
                EnableUserRegister = Configuration.GetSection("SiteSetting:EnableUserRegister").Value,
                GoogleApiKey = Configuration.GetSection("SiteSetting:GoogleApiKey").Value
            });
        }
    }
}
