using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCaliforniaMVCWithData.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace ExploreCaliforniaMVCWithData
{
    public class Startup
    {
        private readonly IConfigurationRoot configuration;
        public Startup(IHostingEnvironment env) 
        {                                     
            configuration = new ConfigurationBuilder()

                                     .AddJsonFile(env.ContentRootPath + "/config.json")
                                    .AddJsonFile(env.ContentRootPath + "/config.dev.json", true)
                                    .AddEnvironmentVariables()
                                    .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Injectable services techniques
            services.AddTransient<SpecialsDataContext>();
            services.AddTransient<FormattingService>();

            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {
                EnableDeveloperExceptions = configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions")

            });

            //configure AddMVC service - middleware needs this service in order to work
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, FeatureToggles features)
        {
            loggerFactory.AddConsole();
            app.UseExceptionHandler("/error.html");
         
            if (features.EnableDeveloperExceptions)                                       
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) => {

                if (context.Request.Path.Value.Contains("invalid"))
                {
                    throw new Exception("ERROR!");
                }
                await next();
            });

            //register mvc middlware in the application
            //install microsoft.aspnetcore.mvc using nuget package manager 
            app.UseMvc(routes => 
            {                                   
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });

            //install static files from nuget
            app.UseFileServer();
        }
    }
}
