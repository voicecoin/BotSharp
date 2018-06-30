using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Voicebot.RestApi
{
    public class WebServicesController : CoreController
    {
        private IApplicationLifetime ApplicationLifetime { get; set; }

        public WebServicesController(IApplicationLifetime appLifetime)
        {
            ApplicationLifetime = appLifetime;
        }

        [HttpGet]
        public IActionResult ShutdownSite()
        {
            ApplicationLifetime.StopApplication();
            return Ok("OK");
        }
    }
}
