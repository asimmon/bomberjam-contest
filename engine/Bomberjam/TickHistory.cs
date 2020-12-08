using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bomberjam
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class TickHistory
    {
        public TickHistory(State state, Dictionary<string, string?> actions)
        {
            this.State = state;
            this.Actions = actions;
        }

        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("actions")]
        public Dictionary<string, string?> Actions { get; set; }
    }
}