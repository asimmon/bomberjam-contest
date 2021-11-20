using Microsoft.Extensions.Hosting;

namespace Bomberjam.Website.Infrastructure
{
    public static class HostEnvironmentExtensions
    {
        public static bool IsLocal(this IHostEnvironment hostEnvironment)
        {
            return hostEnvironment.IsEnvironment("Local");
        }

        public static bool IsLocalOrDevelopment(this IHostEnvironment hostEnvironment)
        {
            return IsLocal(hostEnvironment) || hostEnvironment.IsDevelopment();
        }
    }
}