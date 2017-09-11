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
        public void Initialize(IConfigurationRoot config, IHostingEnvironment env)
        {
        }
    }
}
