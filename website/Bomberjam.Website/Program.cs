using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Bomberjam.Website
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(UseSerilog)
                .ConfigureWebHostDefaults(UseStartup);
        }

        private static void UseSerilog(HostBuilderContext context, ILoggingBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            builder.ClearProviders();
            builder.AddSerilog(Log.Logger);
        }

        private static void UseStartup(IWebHostBuilder webBuilder)
        {
            webBuilder.UseStartup<Startup>();
        }
    }
}
