using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Controllers;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MethodHasAsyncOverload
namespace Bomberjam.Website.Database
{
    public class DatabaseRepository : IRepository
    {
        private readonly BomberjamContext _dbContext;

        public DatabaseRepository(BomberjamContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await this._dbContext.Users.Select(u => MapUser(u)).ToListAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser == null)
                throw new UserNotFoundException($"User '{email}' not found");

            return MapUser(dbUser);
        }

        public async Task<User> GetUserById(int id)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (dbUser == null)
                throw new UserNotFoundException($"User '{id}' not found");

            return MapUser(dbUser);
        }

        private static User MapUser(DbUser dbUser) => new()
        {
            Id = dbUser.Id,
            Created = dbUser.Created,
            Updated = dbUser.Updated,
            Email = dbUser.Email,
            UserName = dbUser.Username,
            GameCount = dbUser.GameCount,
            SubmitCount = dbUser.SubmitCount,
            IsCompiled = dbUser.IsCompiled,
            IsCompiling = dbUser.IsCompiling,
            CompilationErrors = dbUser.CompilationErrors,
            BotLanguage = dbUser.BotLanguage,
        };

        public async Task AddUser(string email, string username)
        {
            this._dbContext.Users.Add(new DbUser
            {
                Email = email,
                Username = username ?? string.Empty,
                GameCount = 0,
                SubmitCount = 0,
                IsCompiled = false,
                IsCompiling = false,
                CompilationErrors = string.Empty,
                BotLanguage = string.Empty,
            });

            await this._dbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(User changedUser)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(e => e.Id == changedUser.Id);
            if (dbUser == null)
                throw new UserNotFoundException($"User '{changedUser.Email ?? changedUser.Id.ToString(CultureInfo.InvariantCulture)}' not found");

            if (!string.IsNullOrWhiteSpace(changedUser.UserName))
                dbUser.Username = changedUser.UserName;

            if (changedUser.GameCount > dbUser.GameCount)
                dbUser.GameCount = changedUser.GameCount;

            if (changedUser.SubmitCount > dbUser.SubmitCount)
                dbUser.SubmitCount = changedUser.SubmitCount;

            dbUser.IsCompiled = changedUser.IsCompiled;
            dbUser.IsCompiling = changedUser.IsCompiling;

            if (changedUser.CompilationErrors != null)
                dbUser.CompilationErrors = changedUser.CompilationErrors;

            dbUser.BotLanguage = string.IsNullOrWhiteSpace(changedUser.BotLanguage)
                ? string.Empty
                : changedUser.BotLanguage;

            await this._dbContext.SaveChangesAsync();
        }

        public async Task<QueuedTask> GetTask(int taskId)
        {
            var dbTask = await this._dbContext.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync();
            if (dbTask == null)
                throw new QueuedTaskNotFoundException($"Task '{taskId}' not found");

            return MapQueuedTask(dbTask);
        }

        public Task AddCompilationTask(int userId)
        {
            var data = userId.ToString(CultureInfo.InvariantCulture);
            return this.AddTask(QueuedTaskType.Compile, data);
        }

        public Task AddGameTask(int[] userIds)
        {
            var data = string.Join(",", userIds.Select(id => id.ToString(CultureInfo.InvariantCulture)));
            return this.AddTask(QueuedTaskType.Game, data);
        }

        private async Task AddTask(QueuedTaskType type, string data)
        {
            this._dbContext.Add(new DbQueuedTask
            {
                Type = type,
                Data = data,
                Status = QueuedTaskStatus.Created,
            });

            await this._dbContext.SaveChangesAsync();
        }

        public async Task<QueuedTask> PopNextTask()
        {
            var dbNextTask = await this._dbContext.Tasks.Where(t => t.Status == QueuedTaskStatus.Created).OrderBy(t => t.Created).FirstOrDefaultAsync();
            if (dbNextTask == null)
                throw new QueuedTaskNotFoundException("There are no more queued tasks");

            dbNextTask.Status = QueuedTaskStatus.Pulled;
            dbNextTask.Updated = DateTime.UtcNow;

            var nextTask = MapQueuedTask(dbNextTask);
            await this._dbContext.SaveChangesAsync();
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

        public async Task MarkTaskAsStarted(int taskId)
        {
            var queuedTask = await this._dbContext.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync();
            if (queuedTask == null)
                throw new QueuedTaskNotFoundException($"The queued task '{taskId}' not found");

            if (queuedTask.Status == QueuedTaskStatus.Pulled)
            {
                queuedTask.Status = QueuedTaskStatus.Started;
                await this._dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"The queued task {taskId} cannot be maked as started because its status is: '{queuedTask.Status}'");
            }
        }

        public async Task MarkTaskAsFinished(int taskId)
        {
            var queuedTask = await this._dbContext.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync();
            if (queuedTask == null)
                throw new QueuedTaskNotFoundException($"The queued task '{taskId}' not found");

            if (queuedTask.Status == QueuedTaskStatus.Started)
            {
                queuedTask.Status = QueuedTaskStatus.Finished;
                await this._dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"The queued task {taskId} cannot be maked as finished because its status is: '{queuedTask.Status}'");
            }
        }

        public async Task<bool> DoesUserHaveActiveCompileTask(int userId)
        {
            var userIdString = userId.ToString(CultureInfo.InvariantCulture);

            return await this._dbContext.Tasks
                .Where(t => t.Type == QueuedTaskType.Compile)
                .Where(t => t.Status != QueuedTaskStatus.Finished)
                .Where(t => t.Data == userIdString)
                .AnyAsync();
        }
    }
}