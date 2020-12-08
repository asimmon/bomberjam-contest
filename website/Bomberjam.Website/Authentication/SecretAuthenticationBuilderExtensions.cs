using System;
using Microsoft.AspNetCore.Authentication;

namespace Bomberjam.Website.Authentication
{
    public static class SecretAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddSecret(this AuthenticationBuilder builder, params string[] secrets)
        {
            if (secrets == null || secrets.Length == 0)
                throw new ArgumentNullException(nameof(secrets));

            return builder.AddScheme<SecretAuthenticationOptions, SecretAuthenticationHandler>(SecretAuthenticationDefaults.AuthenticationScheme, opts =>
            {
                foreach (var secret in secrets)
                {
                    opts.Secrets.Add(secret);
                }
            });
        }
    }
}