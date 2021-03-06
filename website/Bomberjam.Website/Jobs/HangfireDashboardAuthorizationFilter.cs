using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Hangfire.Dashboard;
using Microsoft.Extensions.Configuration;

namespace Bomberjam.Website.Jobs
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly HashSet<string> _allowedGitHubIds;

        public HangfireDashboardAuthorizationFilter(IConfiguration configuration)
        {
            this._allowedGitHubIds = new HashSet<string>(
                (configuration["GitHub:Administrators"] ?? string.Empty).Trim()
                .Split(new[] { ',', ';' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries), StringComparer.Ordinal);
        }

        public bool Authorize(DashboardContext context)
        {
            if (this._allowedGitHubIds.Count == 0) return false;
            var nameIdentifierClaim = context.GetHttpContext().User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return nameIdentifierClaim != null && this._allowedGitHubIds.Contains(nameIdentifierClaim.Value);
        }
    }
}