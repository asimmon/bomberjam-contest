using System;
using Bomberjam.Website.Configuration;
using Microsoft.AspNetCore.Authentication;

namespace Bomberjam.Website.Authentication
{
    public static class SecretAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddSecret(this AuthenticationBuilder builder) => AddSecret(builder, _ => { });

        public static AuthenticationBuilder AddSecret(this AuthenticationBuilder builder, Action<SecretAuthenticationOptions> configure)
        {
            return builder.AddScheme<SecretAuthenticationOptions, SecretAuthenticationHandler>(SecretAuthenticationDefaults.AuthenticationScheme, configure);
        }
    }
}