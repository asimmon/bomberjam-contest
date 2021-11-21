using Bomberjam.Website.Utils;
using Hangfire.Dashboard;

namespace Bomberjam.Website.Jobs
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return context.GetHttpContext().User.IsAdministrator();
        }
    }
}