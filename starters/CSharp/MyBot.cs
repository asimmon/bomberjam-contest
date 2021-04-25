using System;
using System.Linq;
using MyBot.Bomberjam;

namespace MyBot
{
    public class Program
    {
        private static readonly Random Rng = new Random();
        private static readonly ActionKind[] AllActions = { ActionKind.Up, ActionKind.Down, ActionKind.Left, ActionKind.Right, ActionKind.Stay, ActionKind.Bomb };

        public static void Main(string[] args)
        {
            var game = new Game();

            // Standard output (Console.WriteLine) can ONLY BE USED to communicate with the bomberjam process
            // Use text files if you need to log something for debugging
            using (var logger = new Logger())
            {
                // Edit run_game.(bat|sh) to include file logging for any of the four bot processes: dotnet %cd%\bin\Debug\<framework>\MyBot.dll --logging
                if (args.Any(x => "--logging".Equals(x, StringComparison.OrdinalIgnoreCase)))
                    logger.Setup("log-" + DateTime.UtcNow.Ticks + ".log");

                // 1) You must send an alphanumerical name up to 32 characters
                // Spaces or special characters are not allowed
                game.Ready("MyName" + Rng.Next(1000, 9999));
                logger.Info("My player ID is " + game.MyPlayerId);

                do
                {
                    // 2) Each tick, you'll receive the current game state serialized as JSON
                    // From this moment, you have a limited time to send an action back to the bomberjam process through stdout
                    game.ReceiveCurrentState();

                    try
                    {
                        // 3) Analyze the current state and decide what to do
                        for (var x = 0; x < game.State.Width; x++)
                        for (var y = 0; y < game.State.Height; y++)
                        {
                            var tile = game.State.GetTileAt(x, y);
                            if (tile == TileKind.Block)
                            {
                                // TODO found a block to destroy
                            }

                            Player otherPlayer;
                            if (game.State.TryFindAlivePlayerAt(x, y, out otherPlayer) && otherPlayer.Id != game.MyPlayerId)
                            {
                                // TODO found an alive opponent
                            }

                            Bomb bomb;
                            if (game.State.TryFindActiveBombAt(x, y, out bomb))
                            {
                                // TODO found an active bomb
                            }

                            Bonus bonus;
                            if (game.State.TryFindDroppedBonusAt(x, y, out bonus))
                            {
                                // TODO found a bonus
                            }
                        }

                        if (game.MyPlayer.BombsLeft > 0)
                        {
                            // TODO you can drop a bomb
                        }

                        // 4) Send your action
                        var action = AllActions[Rng.Next(AllActions.Length)];
                        game.SendAction(action);
                        logger.Info("Tick " + game.State.Tick + ", sent " + action);
                    }
                    catch (Exception ex)
                    {
                        // Handle your exceptions per tick
                        logger.Error("Tick " + game.State.Tick + ", exception: " + ex.ToString());
                    }
                } while (game.MyPlayer.IsAlive && !game.State.IsFinished);
            }
        }
    }
}