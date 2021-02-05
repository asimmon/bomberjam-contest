using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Bomberjam.Common;

namespace Bomberjam
{
    internal sealed class WorkerOptions
    {
        public WorkerOptions(ProgramArguments args, int currentIteration, FileInfo? output, GameConfiguration? configuration)
        {
            this.PlayerNames = args.PlayerNames.Aggregate(new Dictionary<string, string>(), (acc, name) =>
            {
                acc[acc.Count.ToString(CultureInfo.InvariantCulture)] = name;
                return acc;
            });

            this.PlayerWebsiteIds = args.PlayerWebsiteIds.Aggregate(new Dictionary<string, Guid>(), (acc, id) =>
            {
                acc[acc.Count.ToString(CultureInfo.InvariantCulture)] = id;
                return acc;
            });

            this.Commands = EnsureFourBotCommands(args.Commands);
            this.Quiet = args.IsQuiet;
            this.NoTimeout = args.NoTimeout;
            this.TotalIterations = args.RepeatCount;
            this.Output = output;
            this.Configuration = configuration;
            this.CurrentIteration = currentIteration;
        }

        public IReadOnlyDictionary<string, string> PlayerNames { get; }
        public IReadOnlyDictionary<string, Guid> PlayerWebsiteIds { get; }
        public IReadOnlyCollection<string> Commands { get; }
        public bool Quiet { get; }
        public bool NoTimeout { get; }
        public int CurrentIteration { get; }
        public int TotalIterations { get; }
        public FileInfo? Output { get; }
        public GameConfiguration? Configuration { get; }

        private static IReadOnlyCollection<string> EnsureFourBotCommands(IReadOnlyCollection<string> commands)
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

            return fourBotCommands.ToList();
        }
    }
}