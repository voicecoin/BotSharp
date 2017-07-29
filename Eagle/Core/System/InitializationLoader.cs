using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Core
{
    public class InitializationLoader
    {
        public IHostingEnvironment  Env { get; set; }
        public void Load()
        {
            var loaders = TypeHelper.GetInstanceWithInterface<IInitializationLoader>("Core");
            loaders.OrderBy(x => x.Priority).ToList().ForEach(loader => {
                loader.Initialize(Env);
            });
        }
    }
}
