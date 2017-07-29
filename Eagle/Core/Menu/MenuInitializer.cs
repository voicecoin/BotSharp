using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Utility;

namespace Core.Menu
{
    public class MenuInitializer : IInitializationLoader
    {
        public int Priority => 30;

        public void Initialize(IHostingEnvironment env)
        {
        }
    }
}
