using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Jobs
{
    public sealed class OrphanedTaskFixingJob
    {
        public OrphanedTaskFixingJob(IBomberjamRepository repository, ILogger<MatchmakingJob> logger)
        {
            this.Repository = repository;
            this.Logger = logger;
        }

        private IBomberjamRepository Repository { get; }

        private ILogger<MatchmakingJob> Logger { get; }

        public async Task Run()
        {
            var orphanedTasks = await this.Repository.GetOrphanedTasks();

            var count = 0;
            foreach (var task in orphanedTasks)
            {
                count++;
                if (task.Type == QueuedTaskType.Compile)
                {
                    await this.Repository.MarkTaskAsCreated(task.Id);
                }
                else
                {
                    if (task.Status == QueuedTaskStatus.Pulled)
                    {
                        await this.Repository.MarkTaskAsStarted(task.Id);
                        await this.Repository.MarkTaskAsFinished(task.Id);
                    }
                    else if (task.Status == QueuedTaskStatus.Started)
                    {
                        await this.Repository.MarkTaskAsFinished(task.Id);
                    }
                }
            }

            if (count > 0)
            {
                this.Logger.Log(LogLevel.Information, $"Found {count} orphaned tasks that have been marked as finished");
            }
        }
    }
}