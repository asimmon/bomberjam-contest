using System;
using System.ComponentModel.DataAnnotations;

namespace Bomberjam.Website.Configuration
{
    public sealed class GitHubOptions
    {
        [Required]
        public string ClientId { get; set; } = string.Empty;

        [Required]
        public string ClientSecret { get; set; } = string.Empty;

        [Required]
        public string CallbackPath { get; set; } = string.Empty;

        [Required]
        [NotEmpty]
        public string[] Administrators { get; set; } = Array.Empty<string>();

        [Required]
        public string StarterKitDownloadUrl { get; set; } = string.Empty;
    }
}