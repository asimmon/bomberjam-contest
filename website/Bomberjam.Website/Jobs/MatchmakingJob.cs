using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Jobs
{
    public sealed class MatchmakingJob
    {
        public MatchmakingJob(IBomberjamRepository repository, ILogger<MatchmakingJob> logger)
        {
            this.Repository = repository;
            this.Logger = logger;
        }

        private IBomberjamRepository Repository { get; }

        private ILogger<MatchmakingJob> Logger { get; }

        public async Task Run()
        {
            if (await this.Repository.HasPendingGameTasks())
            {
                this.Logger.Log(LogLevel.Debug, "Skipped matchmaking as there are still game tasks to be processed");
                return;
            }

            var users = await this.Repository.GetUsersWithCompiledBot();
            var matchs = MatchMaker.Execute(users).ToList();

            using (var transaction = await this.Repository.CreateTransaction())
            {
                foreach (var match in matchs)
                    await this.Repository.AddGameTask(match.Users, GameOrigin.RankedMatchmaking);

                await transaction.CommitAsync();
            }

            if (matchs.Count > 0)
            {
                this.Logger.Log(LogLevel.Information, $"Queued {matchs.Count} matches");
            }
        }
    }
}