using Bomberjam.Website.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Bomberjam.Website
{
    public static class Constants
    {
        public const string ApiPrincipalEmail = "api@bomberjam.com";

        public const string SupportedAuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + SecretAuthenticationDefaults.AuthenticationScheme;
    }
}