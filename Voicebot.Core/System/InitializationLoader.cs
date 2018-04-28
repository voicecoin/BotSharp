using DotNetToolkit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Voicebot.Core.Interfaces;

namespace Voicebot.Core
{
    public class InitializationLoader
    {
        public IHostingEnvironment Env { get; set; }
        public IConfiguration Config {get;set;}
        public void Load()
        {
            var appsLoaders = TypeHelper.GetInstanceWithInterface<IInitializationLoader>("Voicebot.Core");
            appsLoaders.ForEach(loader => {
                loader.Initialize(Config, Env);
            });
        }
    }
}
