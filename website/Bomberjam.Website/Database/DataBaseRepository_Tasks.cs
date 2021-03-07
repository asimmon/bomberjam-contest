using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MethodHasAsyncOverload
namespace Bomberjam.Website.Database
{
    public partial class DatabaseRepository
    {
        public async Task<QueuedTask> GetTask(Guid taskId)
        {
            var dbTask = await this._dbContext.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync().ConfigureAwait(false);
            if (dbTask == null)
                throw new EntityNotFound(EntityType.Task, taskId);

            return MapQueuedTask(dbTask);
        }

        public async Task<bool> HasPendingGameTasks()
        {
            // I consider that if there are 4 pending game tasks, they probably come from the matchmaking job
            // Otherwise, there's a chance that the worker is down so let's no queue
            return await this._dbContext.Tasks
                .CountAsync(t => t.Type == QueuedTaskType.Game && t.Status != QueuedTaskStatus.Finished)
                .ConfigureAwait(false) >= 4;
        }

        public Task AddCompilationTask(Guid botId)
        {
            var data = botId.ToString("D");
            return this.AddTask(QueuedTaskType.Compile, data);
        }

        public Task AddGameTask(IReadOnlyCollection<User> users, GameOrigin origin)
        {
            Debug.Assert(users != null && users.Count == 4);

            // <gameOrigin>#<userGuid>:<userName>,<userGuid:userName>,<userGuid:userName>,<userGuid:userName>
            // 1#af05:askaiser,634b:xenure,7ccb:minty,133e:kalmera
            var data = ((int)origin) + "#" + string.Join(",", users.Select(u => $"{u.Id:D}:{u.UserName}"));
            return this.AddTask(QueuedTaskType.Game, data);
        }

        private async Task AddTask(QueuedTaskType type, string data)
        {
            this._dbContext.Add(new DbQueuedTask
            {
                Type = type,
                Data = data,
                Status = QueuedTaskStatus.Created
            });

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<QueuedTask> PopNextTask()
        {
            var dbNextTask = await this._dbContext.Tasks.Where(t => t.Status == QueuedTaskStatus.Created).OrderBy(t => t.Created).FirstOrDefaultAsync().ConfigureAwait(false);
            if (dbNextTask == null)
                throw new EntityNotFound(EntityType.Task);

            dbNextTask.Status = QueuedTaskStatus.Pulled;
            dbNextTask.Updated = DateTime.UtcNow;

            var nextTask = MapQueuedTask(dbNextTask);
            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
            return nextTask;
        }

        private static QueuedTask MapQueuedTask(DbQueuedTask task) => new()
        {
            Id = task.Id,
            Created = task.Created,
            Updated = task.Updated,
            Type = task.Type,
            Status = task.Status,
            Data = task.Data
        };

        public async Task MarkTaskAsCreated(Guid taskId)
        {
            var queuedTask = await this._dbContext.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync().ConfigureAwait(false);
            if (queuedTask == null)
                throw new EntityNotFound(EntityType.Task, taskId);

            queuedTask.Status = QueuedTaskStatus.Created;
            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task MarkTaskAsStarted(Guid taskId)
        {
            var queuedTask = await this._dbContext.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync().ConfigureAwait(false);
            if (queuedTask == null)
                throw new EntityNotFound(EntityType.Task, taskId);

            if (queuedTask.Status == QueuedTaskStatus.Pulled)
            {
                queuedTask.Status = QueuedTaskStatus.Started;
                await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            else
            {
                throw new InvalidOperationException($"The queued task {taskId} cannot be maked as started because its status is: '{queuedTask.Status}'");
            }
        }

        public async Task MarkTaskAsFinished(Guid taskId)
        {
            var queuedTask = await this._dbContext.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync().ConfigureAwait(false);
            if (queuedTask == null)
                throw new EntityNotFound(EntityType.Task, taskId);

            if (queuedTask.Status == QueuedTaskStatus.Started)
            {
                queuedTask.Status = QueuedTaskStatus.Finished;
                await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            else
            {
                throw new InvalidOperationException($"The queued task {taskId} cannot be maked as finished because its status is: '{queuedTask.Status}'");
            }
        }

        public async Task<IEnumerable<QueuedTask>> GetOrphanedTasks()
        {
            // Consider a task as orphaned 20 minutes after it was pulled or started
            var now = DateTime.UtcNow;
            return await this._dbContext.Tasks
                .Where(t => t.Status != QueuedTaskStatus.Created && t.Status != QueuedTaskStatus.Finished)
                .Where(t => t.Updated.AddMinutes(20) < now)
                .OrderByDescending(t => t.Created)
                .Select(t => MapQueuedTask(t))
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}