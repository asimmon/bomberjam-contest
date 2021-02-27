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
                throw new EntityNotFound(ModelType.Task, taskId);

            return MapQueuedTask(dbTask);
        }

        public async Task<bool> HasGameTask()
        {
            return await this._dbContext.Tasks.AnyAsync(t => t.Type == QueuedTaskType.Game && t.Status != QueuedTaskStatus.Finished).ConfigureAwait(false);
        }

        public Task AddCompilationTask(Guid botId)
        {
            var data = botId.ToString("D");
            return this.AddTask(QueuedTaskType.Compile, data);
        }

        public Task AddGameTask(IReadOnlyCollection<User> users)
        {
            Debug.Assert(users != null && users.Count == 4);

            // <userGuid>:<userName>,<userGuid:userName>,<userGuid:userName>,<userGuid:userName>
            var data = string.Join(",", users.Select(u => $"{u.Id:D}:{u.UserName}"));
            return this.AddTask(QueuedTaskType.Game, data);
        }

        private async Task AddTask(QueuedTaskType type, string data, Guid? relatedUserId = null)
        {
            this._dbContext.Add(new DbQueuedTask
            {
                Type = type,
                Data = data,
                Status = QueuedTaskStatus.Created,
                UserId = relatedUserId
            });

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<QueuedTask> PopNextTask()
        {
            var dbNextTask = await this._dbContext.Tasks.Where(t => t.Status == QueuedTaskStatus.Created).OrderBy(t => t.Created).FirstOrDefaultAsync().ConfigureAwait(false);
            if (dbNextTask == null)
                throw new EntityNotFound(ModelType.Task);

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

        public async Task MarkTaskAsStarted(Guid taskId)
        {
            var queuedTask = await this._dbContext.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync().ConfigureAwait(false);
            if (queuedTask == null)
                throw new EntityNotFound(ModelType.Task, taskId);

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
                throw new EntityNotFound(ModelType.Task, taskId);

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
    }
}