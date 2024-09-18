using Edent.Api.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System;
using System.IO;
using System.Reflection;

namespace Edent.Api.Infrastructure.Factories
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                o =>
                {
                    o.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    o.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                });

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
