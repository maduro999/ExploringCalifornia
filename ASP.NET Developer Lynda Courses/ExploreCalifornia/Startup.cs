using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExploreCalifornia
{
    public class Startup
    {
        private readonly IConfigurationRoot configuration;//configuration object set here as a property
        public Startup(IHostingEnvironment env) //IHostingEnvironment refernce needs to be brough in because we are
        {                                       //looking for a rootpath
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
            //services.AddTransient<FeatureToggles>();//creates an instance if the FeatueToggles class
            //you can control how instances are created in core. We instantiate this class with the configuration values

            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {

                EnableDeveloperExceptions = configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions")

            });
            //Now I can reference the configuration object when I create a new instance of the FeatureToggles class.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //The ASP.NET pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, FeatureToggles features)
        {//at runtime the FeatureTogges class gets insyantiated and injected by .net core's dependency injection framework.
        //then it can be used in the code to determine what the dev options are, without knowing or caring where the values came from
        //*see if statement below

            loggerFactory.AddConsole();

            //this middleware function below listens to any exception that may occur then redirects to the error static page
            //of course this gets called first since it's the first middleware in the pipeline
            //this shows the end user a simple error page.
            //to view this error page change the environment in the projects properties from dev to prod
            //or have it read for the json file
            app.UseExceptionHandler("/error.html");

            //to use the configuration api using environment variables - an instance of the 
            //configuration builder object must be created and import
            //its namespace 'using Microsoft.Extensions.Configuration;'
            //--------------------tightly coupled--The configuration object was moved to a property and populated in the Startup ctor
            //Note: Dependency Injection is a development pattern that encourages loue coupliing between components
            //Note: This code below is tightly coupled to the conffiguration API, since is has to know everything 
            //about where these configuration values come from and how to read them from the configuration object.
            ///var configuration = new ConfigurationBuilder()
                                    //another configuration source, using json files. Add a json call in the configuration builder 
                                    //with the path the the json config file, which needs to be created in the root of the project.
                                   /// .AddJsonFile(env.ContentRootPath + "/config.json")
                                    ///.AddJsonFile(env.ContentRootPath + "/config.dev.json", true)//the true flag tells the 
                                    //config api that if a file does not exist then keep on going with the existing config file.
                                    //true is an optional switch
                                    ///.AddEnvironmentVariables()
                                    ///.Build();       

           // if (configuration.GetValue<bool>("FeaturesToggle:EnableDevExceptions"))//this is looking at the json file 
           if(features.EnableDeveloperExceptions)//for this DI to be used the service needs to be added in the ConfigureServices
                                                //which allows you to configure your DI logic
            {
                app.UseDeveloperExceptionPage();
            }
            //---------------------tightly coupled

                /*

                if (configuration["EnableDevMode"]=="false") //this env dev variable was set in the project configurations with a true value
                    //now when I set this to true it will show the dev error page, if I set the env variable to false it will display the error html page
                    //basically setting this to false will skip the registration of the developer exception page 'UseDeveloperExceptionPage'
                {
                    //if the environment is dev then display a more detailed error page - for devs only.
                    app.UseDeveloperExceptionPage();
                }

                */

                app.Use(async (context, next) => {

                if (context.Request.Path.Value.Contains("invalid"))
                {
                    throw new Exception("ERROR!");
                }
                await next();
            });


            /*********************************************************** Middleware
                        app.Use(async (context, next) =>
                        {
                            //if the browser url matches /hello the output msg Hello world2
                            //and then process the next middle ware request, hello world!!!
                            //else, it process the next function Hello world!!!
                            if (context.Request.Path.Value.StartsWith("/hello"))
                            {
                                await context.Response.WriteAsync("Hello World123");
                            }
                            await next();
                        });
                        app.Run(async (context) =>
                        {
                            await context.Response.WriteAsync("Hello World!!!!");
                        });
             ***********************************************************************/


            //Register the StaticFiles middleware component which tells asp net core to try and map
            //any unhandled requests to a file in the wwwroot folder, if that file exists go aheaed and serve it.
            //this mapping even includes defaut pages such as an index.html.
            //You can use the static files middleware to render any kind of static content that you'd like.
            app.UseFileServer();
        }
    }
}
