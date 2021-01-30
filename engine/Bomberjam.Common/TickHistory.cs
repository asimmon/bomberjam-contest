using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bomberjam.Common
{
    public class TickHistory
    {
        public TickHistory(State state, Dictionary<string, string?> actions)
        {
            this.State = state;
            this.Actions = actions;
        }

        [JsonPropertyName("state")]
        public State State { get; set; }

        [JsonPropertyName("actions")]
        public Dictionary<string, string?> Actions { get; set; }
    }
}