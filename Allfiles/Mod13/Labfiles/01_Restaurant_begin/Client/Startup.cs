﻿using Client.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();

            app.UseNodeModules(env.ContentRootPath);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "RestaurantRoute",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "RestaurantBranches", action = "Index" },
                    constraints: new { id = "[0-9]+" });
            });
        }
    }
}
