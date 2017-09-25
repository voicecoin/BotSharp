using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Utility;
using Microsoft.Extensions.Configuration;

namespace Core.Menu
{
    public class MenuInitializer : IInitializationLoader
    {
        public int Priority => 100;

        public void Initialize(IConfiguration config, IHostingEnvironment env)
        {
        }
    }
}
