using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace Bomberjam.Website.Configuration
{
    public sealed class SecretAuthenticationOptions : AuthenticationSchemeOptions
    {
        [Required]
        [NotEmpty]
        public string Secret { get; set; } = string.Empty;
    }
}