using System.Collections.Generic;
using Bomberjam.Website.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace Bomberjam.Website.Logging
{
    public static class UserEnricherExtensions
    {
        public static IApplicationBuilder UseUserEnricher(this IApplicationBuilder builder)
        {
            return builder.UseHttpContextEnricher(GetUserEnrichers);
        }

        private static IEnumerable<ILogEventEnricher> GetUserEnrichers(HttpContext httpContext)
        {
            var userId = httpContext.User.GetUserId();
            if (userId.HasValue)
                yield return new PropertyEnricher(LogPropertyNames.UserId, userId.Value);

            var githubId = httpContext.User.GetGithubId();
            if (githubId != null)
                yield return new PropertyEnricher(LogPropertyNames.GithubId, githubId);
        }
    }
}