using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace Library.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder applicationBuilder, string root)
        {
            var path = Path.Combine(root, "node_modules");
            var fileProvider = new PhysicalFileProvider(path);

            var options = new StaticFileOptions
            {
                RequestPath = "/node_modules",
                FileProvider = fileProvider
            };

            applicationBuilder.UseStaticFiles(options);
            return applicationBuilder;
        }
    }
}
