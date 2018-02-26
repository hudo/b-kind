using System;
using System.Threading.Tasks;
using BKind.Web.Infrastructure.Helpers;
using BKind.Web.Infrastructure.Persistance;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BKind.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            Task.Run(() =>
            {
                using (var scope = host.Services.CreateScope())
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();

                    try
                    {
                        var dbContext = scope.ServiceProvider.GetService<StoriesDbContext>();

                        dbContext.Database.Migrate();

                        logger.LogDebug("Migration done.");
                        
                        dbContext.EnsureDataSeed();

                        logger.LogDebug("Seeding done.");
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Error with data seed: {e.Unwrap().Message}");
                    }
                }
            });

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:8080")
                .Build();
    }
}
