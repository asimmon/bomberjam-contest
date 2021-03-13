using System.Linq;
using System.Security.Claims;
using Bomberjam.Website.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Bomberjam.Website.Jobs
{
    public class AdministrationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAuthenticated = context.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated)
            {
                context.Result = new RedirectToActionResult("Index", "Web", new { });
                return;
            }

            var githubConfiguration = context.HttpContext.RequestServices.GetRequiredService<GitHubConfiguration>();
            if (githubConfiguration.AllowedGitHubIds.Count == 0)
            {
                context.Result = new RedirectToActionResult("Index", "Web", new { });
                return;
            }

            var nameIdentifierClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var isAdministrator = nameIdentifierClaim != null && githubConfiguration.AllowedGitHubIds.Contains(nameIdentifierClaim.Value);
            if (!isAdministrator)
            {
                context.Result = new RedirectToActionResult("Index", "Web", new { });
            }
        }
    }
}