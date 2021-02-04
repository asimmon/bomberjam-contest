using System.Globalization;

namespace Bomberjam.Tests
{
    public static class Extensions
    {
        internal static void AddPlayers(this Simulator simulator, params string[] playerIds)
        {
            foreach (var playerId in playerIds)
            {
                simulator.AddPlayer(playerId, playerId.ToString(CultureInfo.InvariantCulture), null);
            }
        }
    }
}