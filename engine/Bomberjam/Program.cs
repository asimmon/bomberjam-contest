using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using Bomberjam.Common;

namespace Bomberjam
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var parsedArgs = ProgramArguments.Parse(args);
            if (parsedArgs.ShowHelp)
            {
                parsedArgs.WriteHelp(Console.Out);
                return;
            }

            var configuration = GetRequestedConfiguration(parsedArgs.ConfigurationPath);
            configuration.Seed = parsedArgs.Seed;

            for (var iter = 1; iter <= parsedArgs.RepeatCount; iter++)
            {
                var outputFile = GetOutputFile(parsedArgs.OutputPath, iter);
                var opts = new WorkerOptions(parsedArgs, iter, outputFile, configuration);

                using var worker = new Worker(opts);
                worker.Work();
            }
        }

        private static GameConfiguration GetRequestedConfiguration(string? nullableConfigPath)
        {
            if (nullableConfigPath?.Trim() is { } configPath && configPath.Length > 0)
            {
                try
                {
                    var fileContents = File.ReadAllText(configPath);
                    var configuration = JsonSerializer.Deserialize<GameConfiguration>(fileContents);
                    if (configuration == null)
                        throw new Exception($"Could not read configuration file '{configPath}'");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not read configuration file '{configPath}'", ex);
                }
            }

            return new GameConfiguration();
        }

        private static FileInfo? GetOutputFile(string? nullableOutputPathPattern, int gameIteration)
        {
            if (nullableOutputPathPattern?.Trim() is { Length: > 0 } outputPathPattern)
            {
                var outputPath = outputPathPattern.Replace("#n", gameIteration.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase);

                try
                {
                    return new FileInfo(outputPath);
                }
                catch (Exception ex)
                {
                    throw new Exception($"The game output path '{outputPath}' is invalid", ex);
                }
            }

            return null;
        }
    }
}