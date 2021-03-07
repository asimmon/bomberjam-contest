using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Bomberjam.Website.Common
{
    public class GitHubConfiguration
    {
        public GitHubConfiguration(IConfiguration configuration)
        {
            this.AllowedGitHubIds = new HashSet<string>(
                (configuration["GitHub:Administrators"] ?? string.Empty).Trim()
                .Split(new[] { ',', ';' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries), StringComparer.Ordinal);

            this.StartKitsArtifactsUrl = configuration["GitHub:StarterKitsArtifactsUrl"];
            if (string.IsNullOrWhiteSpace(this.StartKitsArtifactsUrl))
                throw new ArgumentException("GitHub:StarterKitsArtifactsUrl configuration is null or empty");
        }

        public HashSet<string> AllowedGitHubIds { get; }

        public string StartKitsArtifactsUrl { get; }
    }
}