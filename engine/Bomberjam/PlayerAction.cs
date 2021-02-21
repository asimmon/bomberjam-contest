using System;

namespace Bomberjam
{
    internal class PlayerAction
    {
        public PlayerAction(string playerId, ActionCode action)
        {
            this.PlayerId = playerId;
            this.Action = action;
            this.Latency = TimeSpan.Zero;
        }

        public string PlayerId { get; }

        public ActionCode Action { get; }

        public TimeSpan Latency { get; set; }
    }
}