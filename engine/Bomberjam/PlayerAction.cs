namespace Bomberjam
{
    internal class PlayerAction
    {
        public PlayerAction(string playerId, ActionCode action)
        {
            this.PlayerId = playerId;
            this.Action = action;
        }

        public string PlayerId { get; }

        public ActionCode Action { get; }
    }
}