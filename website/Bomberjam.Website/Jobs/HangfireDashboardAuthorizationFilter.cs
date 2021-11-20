using System.Linq;
using System.Security.Claims;
using Bomberjam.Website.Configuration;
using Hangfire.Dashboard;
using Microsoft.Extensions.Options;

namespace Bomberjam.Website.Jobs
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly IOptions<GitHubOptions> _githubOptions;

        public HangfireDashboardAuthorizationFilter(IOptions<GitHubOptions> githubOptions)
        {
            this._githubOptions = githubOptions;
        }

        public bool Authorize(DashboardContext context)
        {
            var allowedGithubIds = this._githubOptions.Value.Administrators;
            if (allowedGithubIds.Length == 0) return false;
            var nameIdentifierClaim = context.GetHttpContext().User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return nameIdentifierClaim != null && allowedGithubIds.Contains(nameIdentifierClaim.Value);
        }
    }
}