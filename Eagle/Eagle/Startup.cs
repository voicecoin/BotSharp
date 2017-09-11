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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Core;

namespace Eagle
{
    public partial class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("authsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"authsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("schedulersettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"schedulersettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            CoreController.Configuration = Configuration;
            CoreDbContext.Configuration = Configuration;
            CoreDbContext.Env = env;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ApiExceptionFilter>();

            services.AddCors();

            services.AddAuthentication();

            bool enableMemcached = bool.Parse(Configuration.GetSection("enyimMemcached:Enable").Value);
            if (enableMemcached)
            {
                services.AddEnyimMemcached(options => Configuration.GetSection("enyimMemcached").Bind(options));
            }

            // Add framework services.
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", new MediaTypeHeaderValue("application/xml"));
                options.RespectBrowserAcceptHeader = true;
            }).AddXmlSerializerFormatters()
              .AddXmlDataContractSeria‌​lizerFormatters()
              .AddJsonOptions(options =>
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

            bool enableMemcached = bool.Parse(Configuration.GetSection("enyimMemcached:Enable").Value);
            if (enableMemcached)
            {
                app.UseEnyimMemcached();
            }

            ConfigureAuth(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "API",
                    template: "v1/{controller=Home}/{action=Index}/{id?}");
            });

            // for wwwroot
            app.UseDefaultFiles();

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files
            app.UseStaticFiles(new StaticFileOptions()
            {
                //FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"AdminUI")),
                //RequestPath = new PathString("/AdminUI"),
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }
            });

            InitializationLoader loader = new InitializationLoader();
            loader.Env = env;
            loader.config = Configuration;
            loader.Load();
        }
    }
}
