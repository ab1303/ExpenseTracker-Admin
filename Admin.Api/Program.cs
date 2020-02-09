using System.IO;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;

namespace Admin.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", false, false);
                    config.AddEnvironmentVariables();

                })
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
