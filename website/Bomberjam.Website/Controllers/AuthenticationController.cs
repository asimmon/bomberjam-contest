using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Bomberjam.Website.Storage;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    public class AuthenticationController : BaseBomberjamController<AuthenticationController>
    {
        public AuthenticationController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<AuthenticationController> logger)
            : base(repository, storage, logger)
        {
        }

        [HttpGet("~/signin")]
        public async Task<IActionResult> SignIn()
        {
            var isAuthenticated = this.User?.Identity?.IsAuthenticated ?? false;
            if (isAuthenticated)
            {
                return this.RedirectToAction("Index", "Account");
            }

            return this.View("SignIn", await GetExternalProvidersAsync(this.HttpContext));
        }

        [HttpPost("~/signin")]
        public async Task<IActionResult> SignIn([FromForm] string provider)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                return this.BadRequest();
            }

            if (!await IsProviderSupportedAsync(this.HttpContext, provider))
            {
                return this.BadRequest();
            }

            // Instruct the middleware corresponding to the requested external identity
            // provider to redirect the user agent to its own authorization endpoint.
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs
            return this.Challenge(new AuthenticationProperties { RedirectUri = "/signin-github" }, provider);
        }

        [HttpGet("~/signin-github")]
        public async Task<IActionResult> SignInGithub()
        {
            if (this.TryGetNameIdentifierClaim(out var githubId) && this.TryGetEmailClaim(out var email))
            {
                try
                {
                    await this.Repository.GetUserByGithubId(githubId);
                }
                catch (EntityNotFound)
                {
                    await ComputeAllUserGlobalRanksMutex.WaitAsync();

                    try
                    {
                        using (var transaction = await this.Repository.CreateTransaction())
                        {
                            var temporaryUsername = await this.GenerateUniqueUserName();
                            await this.Repository.AddUser(githubId, email, temporaryUsername);
                            await this.Repository.UpdateAllUserGlobalRanks();
                            await transaction.CommitAsync();
                        }
                    }
                    finally
                    {
                        ComputeAllUserGlobalRanksMutex.Release();
                    }
                }
            }

            return this.RedirectToAction("Index", "Account");
        }

        private async Task<string> GenerateUniqueUserName()
        {
            while (true)
            {
                var userName = "Player" + Constants.Rng.Next(100000, 999999).ToString(CultureInfo.InvariantCulture);
                var isAlreadyUsed = await this.Repository.IsUserNameAlreadyUsed(userName).ConfigureAwait(false);
                if (!isAlreadyUsed)
                    return userName;
            }
        }

        private bool TryGetEmailClaim(out string email)
        {
            var emailClaim = this.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim != null)
            {
                email = emailClaim.Value;
                return true;
            }

            email = null;
            return false;
        }

        [HttpGet("~/signout")]
        [HttpPost("~/signout")]
        public new IActionResult SignOut()
        {
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            return base.SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private static async Task<AuthenticationScheme[]> GetExternalProvidersAsync(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var schemes = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

            var externalProviders = await schemes.GetAllSchemesAsync();
            return externalProviders.Where(scheme => !string.IsNullOrEmpty(scheme.DisplayName)).ToArray();
        }

        private static async Task<bool> IsProviderSupportedAsync(HttpContext context, string providerName)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var externalProviders = await GetExternalProvidersAsync(context);
            return externalProviders.Any(p => string.Equals(p.Name, providerName, StringComparison.OrdinalIgnoreCase));
        }
    }
}