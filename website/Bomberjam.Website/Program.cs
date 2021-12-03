using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
            var loggerBuilder = new LoggerConfiguration()
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console();

            var logzioToken = context.Configuration.GetValue<string>("LogzioToken");
            if (!string.IsNullOrWhiteSpace(logzioToken))
            {
                var logzioListenerUrl = "https://listener.logz.io:8071/?type=app&token=" + logzioToken;
                loggerBuilder = loggerBuilder.WriteTo.LogzIoDurableHttp(logzioListenerUrl);
            }

            Log.Logger = loggerBuilder.CreateLogger();
            builder.ClearProviders();
            builder.AddSerilog(Log.Logger);
        }

        private static void UseStartup(IWebHostBuilder webBuilder)
        {
            webBuilder.UseStartup<Startup>();
        }
    }
}
