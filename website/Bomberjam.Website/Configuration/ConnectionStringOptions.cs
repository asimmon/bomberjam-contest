using System.ComponentModel.DataAnnotations;

namespace Bomberjam.Website.Configuration
{
    public sealed class ConnectionStringOptions
    {
        [Required]
        public string BomberjamContext { get; set; } = string.Empty;

        [Required]
        public string BomberjamStorage { get; set; } = string.Empty;
    }
}