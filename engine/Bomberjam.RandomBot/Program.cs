using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

var rng = new Random();
var allActions = new[] { "up", "down", "left", "right", "stay", "bomb" };

// 1. Send your player name
Console.Out.WriteLine("0:Bot" + rng.Next(1000, 9999));
Console.Out.Flush();

// 2. Retrieve your player id
var myPlayerId = Console.In.ReadLine();
if (!int.TryParse(myPlayerId, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
    throw new Exception("Could not retrieve player ID from stdin");

while (true)
{
    // 3. Retrieve the game current state
    var stateStr = Console.In.ReadLine();
    if (stateStr != null && JsonSerializer.Deserialize<JsonElement>(stateStr) is { } state && state.GetProperty("tick") is { } tick)
    {
        if (myPlayerId == "1" && tick.ToString() == "5")
        {
            throw new Exception("Something wrong");
        }

        try
        {
            // 4. Send your desired action
            var action = allActions[rng.Next(allActions.Length)];
            Console.Out.WriteLine(tick + ":" + action.ToLowerInvariant());
            Console.Out.Flush();
        }
        catch (Exception ex)
        {
            // DO NOT use Console.WriteLine otherwise it will be sent to the bot process
            Debug.WriteLine(ex);
        }
    }
}