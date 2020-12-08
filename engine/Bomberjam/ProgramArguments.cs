using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Mono.Options;

namespace Bomberjam
{
    internal sealed class ProgramArguments
    {
        private readonly OptionSet _options;

        private bool _showHelp;

        private ProgramArguments()
        {
            this._options = new OptionSet
            {
                { "r|repeat=", "The number of games to play", (int x) => this.RepeatCount = x },
                { "o|output=", "Path of saved games, use placeholder #n to insert game number", x => this.OutputPath = x },
                { "t|no-timeout", "Disabe all timeouts for debugging", x => this.NoTimeout = x != null },
                { "c|config=", "Configuration file path", x => this.ConfigurationPath = x },
                { "q|quiet", "Suppress output logging", x => this.IsQuiet = x != null },
                { "h|help", "Show help and exit", x => this._showHelp = x != null }
            };
        }

        public List<string> Commands { get; } = new List<string>();
        public int RepeatCount { get; private set; } = 1;
        public bool IsQuiet { get; private set; } = false;
        public bool NoTimeout { get; private set; } = false;
        public string? OutputPath { get; private set; }
        public string? ConfigurationPath { get; private set; }

        public static ProgramArguments Parse(IEnumerable<string> args)
        {
            return new ProgramArguments().ParseInternal(args);
        }

        private void AddCommand(string? nullableCommand)
        {
            if (nullableCommand is { } command)
            {
                this.Commands.Add(command.Trim());
            }
        }

        private ProgramArguments ParseInternal(IEnumerable<string> args)
        {
            var commands = this._options.Parse(args);

            if (!this._showHelp)
            {
                foreach (var command in commands)
                {
                    this.AddCommand(command);
                }

                this.RepeatCount = Math.Max(1, this.RepeatCount);
                this.OutputPath = this.OutputPath?.Trim();
                this.ConfigurationPath = this.ConfigurationPath?.Trim();
            }

            return this;
        }

        public bool ShowHelp
        {
            get => this._showHelp || (this.Commands.Count == 0 || this.Commands.Count > 4);
        }

        public void WriteHelp(TextWriter output)
        {
            var execName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "bomberjam.exe" : "bomberjam";

            output.WriteLine($"Usage: {execName} [OPTION]... [BOTPATH]...");
            output.WriteLine($"Example: {execName} -q -r 2 -o replay#n.json \"MyBot.exe\" \"node MyBot.js\" \"python MyBot.py\" \"MyBot.bat\"");
            output.WriteLine();
            output.WriteLine("Options:");
            this._options.WriteOptionDescriptions(output);
            output.Flush();
        }
    }
}