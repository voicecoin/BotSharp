using DotNetToolkit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voicebot.WebStarter
{
    public class InitializationLoader
    {
        public IHostingEnvironment Env { get; set; }
        public IConfiguration Config { get; set; }
        public void Load()
        {
            var appsLoaders1 = TypeHelper.GetInstanceWithInterface<Voicebot.Core.Interfaces.IInitializationLoader>("Voicebot.Core");
            appsLoaders1.ForEach(loader => {
                loader.Initialize(Config, Env);
            });

            var appsLoaders2 = TypeHelper.GetInstanceWithInterface<Voiceweb.Auth.Core.Initializers.IInitializationLoader>("Voiceweb.Auth.Core");
            appsLoaders2.ForEach(loader => {
                loader.Initialize();
            });
        }
    }
}
