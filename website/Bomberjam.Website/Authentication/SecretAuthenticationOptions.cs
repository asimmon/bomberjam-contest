using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace Bomberjam.Website.Authentication
{
    public sealed class SecretAuthenticationOptions : AuthenticationSchemeOptions
    {
        public SecretAuthenticationOptions()
        {
            this.Secrets = new HashSet<string>(StringComparer.Ordinal);
        }

        public HashSet<string> Secrets { get; }
    }
}