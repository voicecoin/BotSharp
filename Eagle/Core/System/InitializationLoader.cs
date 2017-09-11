using Core.Interfaces;
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
        public IConfigurationRoot config {get;set;}
        public void Load()
        {
            var coreLoaders = TypeHelper.GetInstanceWithInterface<IInitializationLoader>("Core");
            coreLoaders.ForEach(loader => {
                loader.Initialize(config, Env);
            });

            var appsLoaders = TypeHelper.GetInstanceWithInterface<IInitializationLoader>("Apps");
            appsLoaders.ForEach(loader => {
                loader.Initialize(config, Env);
            });
        }
    }
}
