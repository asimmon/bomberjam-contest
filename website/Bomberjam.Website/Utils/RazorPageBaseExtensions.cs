using System.Linq;
using Bomberjam.Website.Configuration;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bomberjam.Website.Utils
{
    public static class RazorPageBaseExtensions
    {
        public static bool IsAuthenticated(this RazorPageBase page)
        {
            var isAuthenticated = page.User?.Identity?.IsAuthenticated ?? false;
            return isAuthenticated && page.User.TryGetGitHubId(out string _);
        }

        public static bool IsAuthenticated(this RazorPageBase page, out int githubId)
        {
            githubId = 0;
            var isAuthenticated = page.User?.Identity?.IsAuthenticated ?? false;
            return isAuthenticated && page.User.TryGetGitHubId(out githubId);
        }

        public static bool IsAuthenticated(this RazorPageBase page, out string githubId)
        {
            githubId = null;
            var isAuthenticated = page.User?.Identity?.IsAuthenticated ?? false;
            return isAuthenticated && page.User.TryGetGitHubId(out githubId);
        }

        public static bool IsAuthenticatedAdministrator(this RazorPageBase page)
        {
            if (!IsAuthenticated(page, out string githubId))
                return false;

            var gitHubConfiguration = page.ViewContext?.HttpContext?.RequestServices?.GetRequiredService<IOptions<GitHubOptions>>();
            return gitHubConfiguration != null && gitHubConfiguration.Value.Administrators.Contains(githubId);
        }
    }
}