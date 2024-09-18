using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Edent.Api.Infrastructure.Data.Seed
{
    public static class ApplicationDbContextSeed
    {
        public static IHost Migrate(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
                try
                {
                    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                    if (dbContext != null)
                    {
                        dbContext.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    if (logger != null)
                        logger.LogError(ex, "An error occurred on migrating the DB.");
                }
            }
            return host;
        }

        public static IHost Seed(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

                try
                {
                    Task.Run(async () =>
                    {
                        var dataInitializer = serviceProvider.GetRequiredService<IDataInitializer>();
                        await dataInitializer.InitializeAsync(serviceProvider);

                    }).Wait();

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            return host;
        }
    }
}
