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
        public string ArtifactsUsername { get; set; } = string.Empty;

        [Required]
        public string ArtifactsPassword { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int ArtifactsWorkflowId { get; set; }
    }
}