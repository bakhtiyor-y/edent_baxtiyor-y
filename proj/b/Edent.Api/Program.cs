using Edent.Api.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Edent.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build()
                .Migrate()
                .Seed();
            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //.UseUrls("http://*:5500");
                });
    }
}
