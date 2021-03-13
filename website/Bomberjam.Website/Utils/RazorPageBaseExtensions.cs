using Bomberjam.Website.Common;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Bomberjam.Website.Utils
{
    public static class RazorPageBaseExtensions
    {
        public static bool IsAuthenticated(this RazorPageBase page)
        {
            var isAuthenticated = page.User?.Identity?.IsAuthenticated ?? false;
            return isAuthenticated && page.User.Claims.TryGetGitHubId(out string _);
        }

        public static bool IsAuthenticated(this RazorPageBase page, out int githubId)
        {
            githubId = 0;
            var isAuthenticated = page.User?.Identity?.IsAuthenticated ?? false;
            return isAuthenticated && page.User.Claims.TryGetGitHubId(out githubId);
        }

        public static bool IsAuthenticated(this RazorPageBase page, out string githubId)
        {
            githubId = null;
            var isAuthenticated = page.User?.Identity?.IsAuthenticated ?? false;
            return isAuthenticated && page.User.Claims.TryGetGitHubId(out githubId);
        }

        public static bool IsAuthenticatedAdministrator(this RazorPageBase page)
        {
            if (!IsAuthenticated(page, out string githubId))
                return false;

            var gitHubConfiguration = page.ViewContext?.HttpContext?.RequestServices?.GetService<GitHubConfiguration>();
            return gitHubConfiguration != null && gitHubConfiguration.AllowedGitHubIds.Contains(githubId);
        }
    }
}