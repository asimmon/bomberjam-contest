using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

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
            builder.ClearProviders();

            var serilogBuilder = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console();

            var logger = serilogBuilder.CreateLogger();

            Log.Logger = logger;
            builder.AddSerilog(logger);
        }

        private static void UseStartup(IWebHostBuilder webBuilder)
        {
            webBuilder.UseStartup<Startup>();
        }
    }
}
