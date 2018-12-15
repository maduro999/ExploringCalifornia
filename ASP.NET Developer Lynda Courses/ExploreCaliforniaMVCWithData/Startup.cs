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
using Microsoft.EntityFrameworkCore;


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

            //-----------------------------------------------------------------------------------------------------------------------------------------------
            /* This registration is going to be a bit different than the ones that I've done before. Rather than use the add transient or add scope methods 
             * Entity Framework offers its own method called add db context of type t. This helper method configures everything needed to inject 
             * Entity Framework data context and it accepts this function that allows us to define various options for that data context. 
             * Next, I'll need to tell Entity Framework what kind of database I'm connecting to and I'll do this with the use sequel server method 
             * to say that I will be interacting with a Microsoft sequel server database. Support for connecting to Microsoft sequel server database 
             * lives in its own package so I'll need to add a reference to that package for this to work. This method also requires a connection string 
             * that tells Entity Framework how to connect to the target database and to get this connection string, I'll call a method on the configuration object 
             * called get connection string and I'll pass it, the name of the connection string that I'd like to get. The get connection string method is just a 
             * simple way to read the connection string section of the configuration settings.Config json file
             * These configuration lines build up a Db context options object which Entity Framework needs to be able to give to our data context.
             * To do this, I'll add a constructor in th BlogDataContext class that accepts a Db context options object and just passes it to the Db context base class like this.
             */
            services.AddDbContext<BlogDataContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("BlogDataContext");
                options.UseSqlServer(connectionString);
            });
            /*Now that we've created and configured our new data context class, all that's left is to simply inject it into our controllers.
             So, let's open up our blog controller again and start by injecting a instance of the blog data context class into the constructor.
             */
            //-----------------------------------------------------------------------------------------------------------------------------------------------

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
