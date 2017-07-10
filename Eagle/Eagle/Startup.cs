using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Eagle.DomainModels;
using Eagle.DbTables;
using Eagle.DbContexts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Eagle.Core;
using Microsoft.Net.Http.Headers;

namespace Eagle
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            CoreController.Configuration = Configuration;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ApiExceptionFilter>();

            services.AddCors();

            DataContexts.ConnectionString = Configuration.GetConnectionString("DefaultConnection");

            // Add framework services.
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "API",
                    template: "v1/{controller=Home}/{action=Index}/{id?}");
            });

            MapperInitializer.Initialize();
            DbInitializer.Initialize(env);
        }
    }
}
