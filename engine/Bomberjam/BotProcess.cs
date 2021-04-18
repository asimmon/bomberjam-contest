using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Bomberjam
{
    internal sealed class BotProcess : IDisposable
    {
        private const int MaxMissedReads = 10;
        private const int MaxMissedWrites = 10;

        // Only accept process messages that looks like 5:up, 0:mypseudo, etc.
        private static readonly Regex ProcessMessageRegex = new Regex(
            "^(?<tick>[0-9]{1,5}):(?<message>[a-zA-Z0-9\\-_]{1,32})$",
            RegexOptions.Compiled);

        private readonly string _command;
        private readonly BlockingCollection<ProcessMessage> _messages;
        private readonly StringBuilder _errorLines;
        private readonly StringBuilder _discardedLines;
        private readonly ConcurrentDictionary<int, bool> _readLinesSucceeded;
        private readonly ConcurrentDictionary<int, bool> _writeLinesSucceeded;
        private readonly Process _process;

        public BotProcess(string command)
        {
            this._command = command;

            var processFileName = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "/bin/bash" : "cmd.exe";
            var processArgs = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? $"-c \"{command}\"" : $"/u /q /c \"{command}\"";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = processFileName,
                Arguments = processArgs,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            this._messages = new BlockingCollection<ProcessMessage>();
            this._errorLines = new StringBuilder();
            this._discardedLines = new StringBuilder();
            this._readLinesSucceeded = new ConcurrentDictionary<int, bool>();
            this._writeLinesSucceeded = new ConcurrentDictionary<int, bool>();

            this._process = new Process
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            this._process.OutputDataReceived += this.OnOutputDataReceived;
            this._process.ErrorDataReceived += this.OnErrorDataReceived;
            this._process.Exited += this.OnExited;
        }

        public bool HasExited { get; private set; }

        public string Error
        {
            get => this._discardedLines.ToString() + this._errorLines.ToString();
        }

        public void Start()
        {
            try
            {
                this._process.Start();
            }
            catch (Exception ex)
            {
                this._messages.CompleteAdding();
                throw new Exception($"Failed to start the process '{this._command}'", ex);
            }

            this._process.BeginOutputReadLine();
            this._process.BeginErrorReadLine();
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs args)
        {
            if (args.Data == null)
            {
                this._messages.CompleteAdding();
                return;
            }

            var stdinLine = args.Data.Trim();
            if (ProcessMessageRegex.Match(stdinLine) is { Success: true } match)
            {
                var tick = int.Parse(match.Groups["tick"].Value, NumberStyles.Integer);
                this._messages.Add(new ProcessMessage(tick, match.Groups["message"].Value));
            }
            else if (!string.IsNullOrWhiteSpace(stdinLine))
            {
                this._messages.Add(new ProcessMessage(stdinLine));
            }
        }

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                this._errorLines.AppendLine(e.Data.TrimEnd());
            }
        }

        private void OnExited(object? sender, EventArgs e)
        {
            this.HasExited = true;
            this._messages.CompleteAdding();
        }

        public void WriteLine(int tick, string text)
        {
            if (this.HasExited)
            {
                return;
            }

            this._writeLinesSucceeded[tick] = false;

            if (this._writeLinesSucceeded.Values.Count(written => !written) >= MaxMissedWrites)
            {
                this._errorLines.Append("Process was killed after trying ").Append(MaxMissedWrites).AppendLine(" times to communicate with standard input.");
                this._process.Kill();
                return;
            }

            try
            {
                this._process.StandardInput.WriteLine(text);
                this._writeLinesSucceeded[tick] = true;
            }
            catch
            {
                // ignored
            }
        }

        public ProcessMessage? ReadLineForTick(int tick, CancellationToken token)
        {
            if (this.HasExited)
            {
                return null;
            }

            this._readLinesSucceeded[tick] = false;

            if (this._readLinesSucceeded.Values.Count(read => !read) >= MaxMissedReads)
            {
                this._errorLines.Append("Process was killed after trying ").Append(MaxMissedReads).AppendLine(" times to communicate with standard output.");
                this._process.Kill();
                return null;
            }

            try
            {
                do
                {
                    var message = this._messages.Take(token);
                    if (message.Tick.HasValue)
                    {
                        this._readLinesSucceeded[tick] = true;
                        if (message.Tick.Value == tick)
                            return message;
                    }

                    this._discardedLines.AppendLine($"[{tick}] {message.Message}");
                }
                while (!token.IsCancellationRequested);

                return null;
            }
            catch (InvalidOperationException)
            {
                // Messages queue closed because of process exited / errored / killed
                return null;
            }
            catch (OperationCanceledException)
            {
                // Took too long to retrieve a message from stdout
                return null;
            }
        }

        public void Dispose()
        {
            this._process.Exited -= this.OnExited;
            this._process.ErrorDataReceived -= this.OnErrorDataReceived;
            this._process.OutputDataReceived -= this.OnOutputDataReceived;

            if (!this._process.HasExited)
            {
                try
                {
                    this._process.Kill();
                }
                catch
                {
                    // ignored
                }
            }

            this._process.Dispose();

            this._messages.CompleteAdding();
            this._messages.Dispose();
        }
    }
}