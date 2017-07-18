using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Core.Interfaces;
using Eagle.DataContexts;

namespace Eagle.Core.Menu
{
    public class Hooks : IDbInitializer
    {
        public int Priority => 1;

        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            
        }
    }
}
