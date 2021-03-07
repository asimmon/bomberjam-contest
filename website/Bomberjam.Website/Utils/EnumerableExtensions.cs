using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Bomberjam.Website.Utils
{
    public static class EnumerableExtensions
    {
        public static string JoinStrings(this IEnumerable<string> texts, string separator)
        {
            return string.Join(separator, texts);
        }

        public static bool TryGetGitHubId(this IEnumerable<Claim> claims, out string githubId)
        {
            var nameIdentifierClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim != null && int.TryParse(nameIdentifierClaim.Value, out _))
            {
                githubId = nameIdentifierClaim.Value;
                return true;
            }

            githubId = null;
            return false;
        }

        public static bool TryGetGitHubId(this IEnumerable<Claim> claims, out int githubId)
        {
            if (TryGetGitHubId(claims, out string githubIdStr))
            {
                githubId = int.Parse(githubIdStr);
                return true;
            }

            githubId = -1;
            return false;
        }
    }
}