using System.Security.Claims;

namespace Bomberjam.Website.Utils
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAuthenticated(this ClaimsPrincipal principal)
        {
            return principal.Identity?.IsAuthenticated ?? false;
        }

        public static bool TryGetGitHubUserName(this ClaimsPrincipal principal, out string githubUserName)
        {
            githubUserName = principal.FindFirstValue(ClaimTypes.Name);
            return githubUserName != null;
        }

        public static bool TryGetGitHubId(this ClaimsPrincipal principal, out string githubId)
        {
            var githubIdStr = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (githubIdStr != null && int.TryParse(githubIdStr, out _))
            {
                githubId = githubIdStr;
                return true;
            }

            githubId = null;
            return false;
        }

        public static bool TryGetGitHubId(this ClaimsPrincipal principal, out int githubId)
        {
            if (TryGetGitHubId(principal, out string githubIdStr))
            {
                githubId = int.Parse(githubIdStr);
                return true;
            }

            githubId = -1;
            return false;
        }
    }
}