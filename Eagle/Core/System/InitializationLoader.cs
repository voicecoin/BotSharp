using Core.Interfaces;
using DotNetToolkit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace Core
{
    public class InitializationLoader
    {
        public IHostingEnvironment Env { get; set; }
        public IConfiguration Config {get;set;}
        public void Load()
        {
            var appsLoaders = TypeHelper.GetInstanceWithInterface<IInitializationLoader>("Core", "Apps");
            appsLoaders.ForEach(loader => {
                loader.Initialize(Config, Env);
            });
        }
    }
}
