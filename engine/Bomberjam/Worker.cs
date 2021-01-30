using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Bomberjam.Common;

namespace Bomberjam
{
    internal sealed class Worker : IDisposable
    {
        private static readonly Random Rng = new();

        private static readonly ISet<string> ValidPlayerActions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Constants.Up, Constants.Down, Constants.Left, Constants.Right, Constants.Stay, Constants.Bomb
        };

        private readonly WorkerOptions _opts;
        private readonly string[] _botCommands;
        private readonly Simulator _simulator;
        private readonly IDictionary<string, BotProcess> _processes;
        private readonly List<string> _playerIds;
        private readonly Stopwatch _watch;
        private readonly TimeSpan _playerInitialisationDuration;
        private readonly TimeSpan _tickDuration;

        public Worker(WorkerOptions opts)
        {
            this._opts = opts;

            this._botCommands = EnsureFourBotCommands(opts.Commands);
            this._simulator = new Simulator(opts.Configuration);
            this._processes = new Dictionary<string, BotProcess>(4);
            this._playerIds = new List<string>();
            this._watch = new Stopwatch();
            this._playerInitialisationDuration = this._opts.NoTimeout ? TimeSpan.MaxValue : TimeSpan.FromMinutes(1);
            this._tickDuration = this._opts.NoTimeout ? TimeSpan.MaxValue : TimeSpan.FromSeconds(2);
        }

        private static string[] EnsureFourBotCommands(IReadOnlyCollection<string> commands)
        {
            if (commands.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(commands), "At least one bot command is required");
            }

            if (commands.Count > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(commands), "A maximum of four bot commands can be specified");
            }

            var fourBotCommands = commands.ToList();

            while (fourBotCommands.Count < 4)
            {
                fourBotCommands.Add(fourBotCommands[^1]);
            }

            return fourBotCommands.ToArray();
        }

        public void Work()
        {
            this.CreateProcesses();
            this.AddPlayers();
            this.ExecuteTicks();
            this.SendFinalState();
            this.SaveHistoryOutput();
        }

        private void SaveHistoryOutput()
        {
            if (this._opts.Output is { Directory: { } parentDir } file)
            {
                try
                {
                    parentDir.Create();

                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    using (var fileStream = File.OpenWrite(file.FullName))
                    using (var jsonWriter = new Utf8JsonWriter(fileStream, Constants.DefaultJsonWriterOptions))
                    {
                        JsonSerializer.Serialize(jsonWriter, this._simulator.History);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not save game history to file", ex);
                }
            }
        }

        private void CreateProcesses()
        {
            this._watch.Start();

            if (this._opts.TotalIterations > 1)
            {
                Debug(this._simulator.State.Tick, null, $"================ Game {this._opts.CurrentIteration} / {this._opts.TotalIterations} begins ================");
            }

            for (var playerId = 0; playerId < this._botCommands.Length; playerId++)
            {
                var botCommand = this._botCommands[playerId];

                // Delaying the execution of each process helps randomness to differ from one process to another
                Thread.Sleep(Rng.Next(100, 200));
                var process = new BotProcess(botCommand);
                this._processes[playerId.ToString(CultureInfo.InvariantCulture)] = process;
                process.Start();
            }
        }

        private void AddPlayers()
        {
            var playerNames = new ConcurrentDictionary<string, string>();

            using (var threadGroup = new ThreadGroup())
            {
                foreach (var (playerId, process) in this._processes)
                {
                    var threadState = new ThreadState(playerId.ToString(CultureInfo.InvariantCulture), this._simulator.State, process, playerNames);
                    threadGroup.ExecuteThread(this.AddPlayer, threadState);
                }

                Debug(this._simulator.State.Tick, null, "Waiting for all players to send their names");
                threadGroup.WaitForCompletion(this._playerInitialisationDuration);
                Debug(this._simulator.State.Tick, null, $"Received {playerNames.Count} names in {threadGroup.Elapsed.TotalSeconds:F4} seconds");
            }

            var playerNameUses = playerNames.Aggregate(new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase), (acc, kvp) =>
            {
                var (_, playerName) = kvp;
                acc[playerName] = acc.TryGetValue(playerName, out var uses) ? uses + 1 : 1;
                return acc;
            });

            var uniquePlayerNames = new HashSet<string>(playerNameUses.Keys);

            foreach (var uniquePlayerName in uniquePlayerNames)
            {
                if (playerNameUses.TryGetValue(uniquePlayerName, out var uses) && uses == 1)
                {
                    playerNameUses.Remove(uniquePlayerName);
                }
            }

            foreach (var (playerId, playerName) in playerNames)
            {
                string suffixedUniquePlayerName;
                if (playerNameUses.TryGetValue(playerName, out var uses))
                {
                    suffixedUniquePlayerName = $"{playerName} ({uses})";
                    playerNameUses[playerName]--;
                }
                else
                {
                    suffixedUniquePlayerName = playerName;
                }

                this._simulator.AddPlayer(playerId.ToString(CultureInfo.InvariantCulture), suffixedUniquePlayerName);
                this._playerIds.Add(playerId);
            }
        }

        private void AddPlayer(ThreadState x, CancellationToken threadGroupToken)
        {
            var playerName = x.Process.ReadLineForTick(0, threadGroupToken);
            if (playerName == null || x.Process.HasExited)
            {
                throw new Exception($"Player {x.PlayerId} did not sent a name in time");
            }

            if (playerName.Tick != 0)
            {
                throw new Exception($"Player {x.PlayerId} sent its name with a tick non-equal to zero: {playerName.Tick}");
            }

            var sanitizePlayerName = SanitizePlayerName(x.PlayerId, playerName.Message);
            this.Debug(x, $"Read player name: {sanitizePlayerName}");
            x.ProcessOutput[x.PlayerId] = sanitizePlayerName;

            this.Debug(x, "Sending player ID");
            x.Process.WriteLine(x.PlayerId.ToString(CultureInfo.InvariantCulture));
        }

        private static string SanitizePlayerName(string playerId, string playerName)
        {
            var sanitizedPlayerName = new string(playerName.Trim().Where(c => !char.IsControl(c) && c != '\r' && c != '\n').Take(32).ToArray());
            return sanitizedPlayerName.Length == 0 ? "Player " + playerId : sanitizedPlayerName;
        }

        private void ExecuteTicks()
        {
            while (!this._simulator.State.IsFinished)
            {
                this.ExecuteTick();
            }
        }

        private void ExecuteTick()
        {
            var playerActions = new ConcurrentDictionary<string, string>();

            using (var threadGroup = new ThreadGroup())
            {
                this._playerIds.ShuffleInPlace();

                foreach (var (playerId, process) in this._processes)
                {
                    if (process.HasExited)
                    {
                        this.HandleExitedProcess(playerId);
                    }
                    else if (this._simulator.State.Players.TryGetValue(playerId, out var player) && !player.IsAlive)
                    {
                        // Dead players can no longer play
                    }
                    else
                    {
                        var threadState = new ThreadState(playerId, this._simulator.State, process, playerActions);
                        threadGroup.ExecuteThread(this.ExecuteTick, threadState);
                    }
                }

                Debug(this._simulator.State.Tick, null, "Waiting for all players to send their actions");
                threadGroup.WaitForCompletion(this._tickDuration);

                var validPlayerActions = new Dictionary<string, string>();

                foreach (var (playerId, _) in this._processes)
                {
                    if (playerActions.TryGetValue(playerId, out var playerAction))
                    {
                        if (ValidPlayerActions.Contains(playerAction))
                        {
                            validPlayerActions[playerId] = playerAction;
                        }
                    }
                    else if (this._processes[playerId].HasExited)
                    {
                        this.HandleExitedProcess(playerId);
                    }
                }

                Debug(this._simulator.State.Tick, null, $"Executing tick with {validPlayerActions.Count} valid player actions after waiting {threadGroup.Elapsed.TotalSeconds:F4} seconds");
                this._simulator.ExecuteTick(validPlayerActions);

                Debug(this._simulator.State.Tick, null, "Tiles: " + this._simulator.State.Tiles);
            }
        }

        private void HandleExitedProcess(string playerId)
        {
            if (this._simulator.State.Players.TryGetValue(playerId, out var player) && player.IsAlive)
            {
                Debug(this._simulator.State.Tick, playerId, "Process has exited, killing player");
                this._simulator.KillPlayer(playerId);

                var error = this._processes[playerId].Error.Trim();
                if (error.Length > 0)
                {
                    this._simulator.History.AddError(playerId, this._simulator.State.Tick, error);
                    Debug(this._simulator.State.Tick, playerId, error);
                }
            }
        }

        private void ExecuteTick(ThreadState x, CancellationToken threadGroupToken)
        {
            var watch = new Stopwatch();
            x.Process.WriteLine(x.GameState.ToJson());

            watch.Start();
            var action = x.Process.ReadLineForTick(x.GameState.Tick, threadGroupToken);
            watch.Stop();

            if (action == null)
            {
                this.Debug(x, "Did not receive action in time");
            }
            else
            {
                this.Debug(x, $"Read player action: {action.Message} ({watch.Elapsed.TotalSeconds:F4} seconds)");
                x.ProcessOutput[x.PlayerId] = action.Message;
            }
        }

        private void SendFinalState()
        {
            Debug(this._simulator.State.Tick, null, "Sending final game state");

            using (var threadGroup = new ThreadGroup())
            {
                foreach (var (playerId, process) in this._processes)
                {
                    if (!process.HasExited)
                    {
                        var threadState = new ThreadState(playerId, this._simulator.State, process, new ConcurrentDictionary<string, string>());
                        threadGroup.ExecuteThread(SendFinalState, threadState);
                    }
                }

                threadGroup.WaitForCompletion(this._tickDuration);
            }

            var endScoresMessage = string.Join(", ", this._simulator.State.Players.Values.Select(p => p.Name + ": " + p.Score.ToString(CultureInfo.InvariantCulture)));
            Debug(this._simulator.State.Tick, null, $"Scores: {endScoresMessage}");

            var endGameMessage = this._simulator.State.Players.Values.Where(p => p.HasWon).Select(p => p.Name).FirstOrDefault() is { } winner
                ? $"Game ended, '{winner}' is the last player alive"
                : "Game ended, no winner";

            Debug(this._simulator.State.Tick, null, endGameMessage);

            this._watch.Stop();
            Debug(this._simulator.State.Tick, null, $"Game {this._opts.CurrentIteration} / {this._opts.TotalIterations} ended after {this._watch.Elapsed.TotalSeconds:F4} seconds");
        }

        private static void SendFinalState(ThreadState x, CancellationToken _)
        {
            x.Process.WriteLine(x.GameState.ToJson());
        }

        public void Dispose()
        {
            foreach (var process in this._processes.Values)
            {
                process.Dispose();
            }

            this._processes.Clear();
        }

        private void Debug(int? tick, string? playerId, string text)
        {
            if (!this._opts.Quiet)
            {
                var tickStr = tick.HasValue ? tick.Value.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0') : "   ";
                var playerIdStr = playerId ?? " ";

                Console.Out.WriteLine($"[{DateTime.Now:yyyy-mm-dd HH:mm:ss:ffff}][tick:{tickStr}][player:{playerIdStr}] {text}");
                Console.Out.Flush();
            }
        }

        private void Debug(ThreadState x, string text)
        {
            Debug(x.GameState.Tick, x.PlayerId, text);
        }

        private sealed class ThreadState
        {
            public ThreadState(string playerId, State gameState, BotProcess process, ConcurrentDictionary<string, string> processOutput)
            {
                this.PlayerId = playerId;
                this.GameState = gameState;
                this.Process = process;
                this.ProcessOutput = processOutput;
            }

            public string PlayerId { get; }

            public State GameState { get; }

            public BotProcess Process { get; }

            public ConcurrentDictionary<string, string> ProcessOutput { get; }
        }
    }
}