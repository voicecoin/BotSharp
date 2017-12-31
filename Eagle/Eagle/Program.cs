using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Core;

namespace Eagle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => {
                    
                    var env = hostingContext.HostingEnvironment;
                    var settings = Directory.GetFiles("Settings", "*settings.json");
                    settings.ToList().ForEach(setting => {
                        config.AddJsonFile(setting, optional: false, reloadOnChange: true);
                    });
                })
                .UseStartup<Startup>()
                .Build();
    }
}
