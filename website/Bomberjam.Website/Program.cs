using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(ConfigureWebHostDefaults);
        }

        private static void ConfigureWebHostDefaults(IWebHostBuilder webBuilder)
        {
            webBuilder.UseStartup<Startup>();
        }
    }
}
