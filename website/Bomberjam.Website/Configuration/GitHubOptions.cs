using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using GSoft.ComponentModel.DataAnnotations;

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

        public string ArtifactsUsername { get; set; }

        public string ArtifactsPassword { get; set; }

        [Range(1, int.MaxValue)]
        public int ArtifactsWorkflowId { get; set; }
    }
}