using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bomberjam.Common
{
    public class GameHistory
    {
        public GameHistory()
        {
        }

        public GameHistory(GameConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        [JsonPropertyName("configuration")]
        public GameConfiguration Configuration { get; set; } = new GameConfiguration();

        [JsonPropertyName("summary")]
        public GameSummary Summary { get; set; } = new GameSummary();

        [JsonPropertyName("ticks")]
        public IList<TickHistory> Ticks { get; set; } = new List<TickHistory>();
    }
}