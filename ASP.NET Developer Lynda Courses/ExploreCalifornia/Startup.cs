using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExploreCalifornia
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //The ASP.NET pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            //this middleware function listens to any exception that may occur then redirects to the error static page
            //of course this gets calles first since it's the first middleware in the pipeline
            //this shows the end user a simple error page.
            //to view this error page change the environment in the projects properties from dev to prod
            app.UseExceptionHandler("/error.html");

            if (env.IsDevelopment())
            {
                //if the environment is dev then display a more detailed error page - for devs only.
                app.UseDeveloperExceptionPage();
            }



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
            app.Use(async (context, next) => {

                if (context.Request.Path.Value.StartsWith("/hello"))
                {
                    throw new Exception("ERROR!");
                }
                await next();
            });

            //Register the StaticFiles middleware component which tells asp net core to try and map
            //any unhandled requests to a file in the wwwroot folder, if that file exists go aheaed and serve it.
            //this mapping even includes defaut pages such as an index.html.
            //You can use the static files middleware to render any kind of static content that you'd like.
            app.UseFileServer();
        }
    }
}
