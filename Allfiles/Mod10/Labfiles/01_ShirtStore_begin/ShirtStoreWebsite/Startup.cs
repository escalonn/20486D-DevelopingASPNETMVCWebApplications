﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShirtStoreWebsite.Data;
using ShirtStoreWebsite.Services;

namespace ShirtStoreWebsite
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ShirtContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IShirtRepository, ShirtRepository>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ShirtContext shirtContext)
        {
            shirtContext.Database.EnsureDeleted();
            shirtContext.Database.EnsureCreated();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultRoute",
                    template: "{controller=Shirt}/{action=Index}/{id?}");
            });
        }
    }
}
