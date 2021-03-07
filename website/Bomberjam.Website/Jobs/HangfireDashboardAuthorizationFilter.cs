using System.Linq;
using System.Security.Claims;
using Bomberjam.Website.Common;
using Hangfire.Dashboard;

namespace Bomberjam.Website.Jobs
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly GitHubConfiguration _configuration;

        public HangfireDashboardAuthorizationFilter(GitHubConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public bool Authorize(DashboardContext context)
        {
            if (this._configuration.AllowedGitHubIds.Count == 0) return false;
            var nameIdentifierClaim = context.GetHttpContext().User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return nameIdentifierClaim != null && this._configuration.AllowedGitHubIds.Contains(nameIdentifierClaim.Value);
        }
    }
}