using System.ComponentModel.DataAnnotations;

namespace Bomberjam.Website.Configuration
{
    public sealed class JobOptions
    {
        [Required]
        public string Matchmaking { get; set; } = string.Empty;

        [Required]
        public string OrphanedTasks { get; set; } = string.Empty;

        [Required]
        public string GithubArtifacts { get; set; } = string.Empty;
    }
}