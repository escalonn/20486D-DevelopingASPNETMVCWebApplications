using Library.Data;
using Library.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library
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
            services.AddDbContext<LibraryContext>(options =>
                 options.UseSqlite("Data Source=library.db"));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, LibraryContext libraryContext)
        {
            libraryContext.Database.EnsureDeleted();
            libraryContext.Database.EnsureCreated();

            app.UseStaticFiles();

            app.UseNodeModules(env.ContentRootPath);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "LibraryRoute",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Library", action = "Index" },
                    constraints: new { id = "[0-9]+" });
            });
        }
    }
}
