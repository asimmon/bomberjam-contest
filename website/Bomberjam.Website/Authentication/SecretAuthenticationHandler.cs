using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Bomberjam.Website.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Bomberjam.Website.Authentication
{
    public sealed class SecretAuthenticationHandler : AuthenticationHandler<SecretAuthenticationOptions>
    {
        public SecretAuthenticationHandler(IOptionsMonitor<SecretAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var requiredSecret = this.OptionsMonitor.CurrentValue.Secret;
            if (requiredSecret.Length == 0)
                return Task.FromResult(AuthenticateResult.NoResult());

            if (!this.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizationHeaderValues))
                return Task.FromResult(AuthenticateResult.NoResult());

            var authorizationHeader = authorizationHeaderValues.ToString();
            if (string.IsNullOrEmpty(authorizationHeader))
                return Task.FromResult(AuthenticateResult.NoResult());

            if (!authorizationHeader.StartsWith(SecretAuthenticationDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.NoResult());

            var secret = authorizationHeader[SecretAuthenticationDefaults.AuthenticationScheme.Length..].TrimStart();
            if (!string.Equals(requiredSecret, secret, StringComparison.Ordinal))
                return Task.FromResult(AuthenticateResult.Fail("Invalid authentication secret"));

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, Constants.ApiPrincipalFakeEmail)
            };

            var identity = new ClaimsIdentity(claims, this.Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}