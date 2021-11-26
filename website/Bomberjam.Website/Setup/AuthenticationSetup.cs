using AspNet.Security.OAuth.GitHub;
using Bomberjam.Website.Authentication;
using Bomberjam.Website.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Bomberjam.Website.Setup
{
    public class AuthenticationSetup :
        IConfigureOptions<AuthenticationOptions>,
        IConfigureOptions<AuthorizationOptions>,
        IConfigureNamedOptions<CookieAuthenticationOptions>,
        IConfigureNamedOptions<GitHubAuthenticationOptions>
    {
        private readonly IOptions<GitHubOptions> _githubOptions;

        public AuthenticationSetup(IOptions<GitHubOptions> githubOptions)
        {
            this._githubOptions = githubOptions;
        }

        public void Configure(AuthenticationOptions options)
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }

        public void Configure(AuthorizationOptions options)
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(BomberjamClaimTypes.UserId)
                .Build();
        }

        public void Configure(string name, CookieAuthenticationOptions options) => this.Configure(options);

        public void Configure(CookieAuthenticationOptions options)
        {
            options.LoginPath = "/signin";
            options.LogoutPath = "/signout";
            options.AccessDeniedPath = "/access-denied";
            options.ReturnUrlParameter = "returnUrl";
        }

        public void Configure(string name, GitHubAuthenticationOptions options) => this.Configure(options);

        public void Configure(GitHubAuthenticationOptions options)
        {
            options.ClientId = this._githubOptions.Value.ClientId;
            options.ClientSecret = this._githubOptions.Value.ClientSecret;
            options.CallbackPath = this._githubOptions.Value.CallbackPath;
        }
    }
}