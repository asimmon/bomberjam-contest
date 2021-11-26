using System;
using System.Security.Claims;
using Bomberjam.Website.Authentication;

namespace Bomberjam.Website.Utils
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAuthenticated(this ClaimsPrincipal principal)
        {
            return principal.Identity?.IsAuthenticated ?? false;
        }

        public static bool IsAdministrator(this ClaimsPrincipal principal)
        {
            return IsAuthenticated(principal) && principal.IsInRole(BomberjamRoles.Admin);
        }

        public static Guid? GetUserId(this ClaimsPrincipal principal)
        {
            if (!IsAuthenticated(principal)) return null;
            var userIdStr = principal.FindFirstValue(BomberjamClaimTypes.UserId);
            return Guid.TryParse(userIdStr, out var userId) ? userId : null;
        }

        public static string GetGithubId(this ClaimsPrincipal principal)
        {
            return IsAuthenticated(principal) ? principal.FindFirstValue(BomberjamClaimTypes.GithubId) ?? string.Empty : string.Empty;
        }
    }
}