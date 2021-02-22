using System;
using System.Text.Json;

namespace MyBot
{
    public class Program
    {
        private static readonly Random Rng = new Random();
        private static readonly string[] AllActions = { "up", "down", "left", "right", "stay", "bomb" };

        public static void Main()
        {
            // Standard output (Console.WriteLine) can ONLY BE USED to communicate with the bomberjam process
            // Use text files if you need to log something for debugging

            // 1) You must send an alphanumerical name up to 32 characters, prefixed by "0:"
            // No spaces or special characters are allowed
            Console.Out.WriteLine("0:MyName" + Rng.Next(1000, 9999));
            Console.Out.Flush();

            // 2) Receive your player ID from the standard input
            var myPlayerId = Console.In.ReadLine();

            while (true)
            {
                // 3) Each tick, you'll receive the current game state serialized as JSON
                // From this moment, you have a limited time to send an action back to the bomberjam process through stdout
                var state = JsonSerializer.Deserialize<JsonElement>(Console.In.ReadLine());

                try
                {
                    // 4) Send your action prefixed by the current tick number and a colon
                    var action = AllActions[Rng.Next(AllActions.Length)];
                    Console.Out.WriteLine(state.GetProperty("tick") + ":" + action);
                    Console.Out.Flush();
                }
                catch
                {
                    // Handle your exceptions per tick
                }
            }
        }
    }
}