using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ShirtStoreWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    IHostingEnvironment env = hostingContext.HostingEnvironment;
                    IConfigurationSection config = hostingContext.Configuration.GetSection("Logging");
                    logging.ClearProviders();

                    if (env.IsDevelopment())
                    {
                        logging.AddConsole();
                    }
                    else
                    {
                        logging.AddFile(config);
                    }
                })
                .UseStartup<Startup>();
    }
}
