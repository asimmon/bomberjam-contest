using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Common;
using Bomberjam.Website.Common;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MethodHasAsyncOverload
namespace Bomberjam.Website.Database
{
    public class DatabaseRepository : IRepository
    {
        private const int UserGamesPageItemsCount = 25;

        private readonly BomberjamContext _dbContext;

        public DatabaseRepository(BomberjamContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await this._dbContext.Users.Select(u => MapUser(u)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<User> GetUserByGithubId(int githubId)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(u => u.GithubId == githubId).ConfigureAwait(false);
            if (dbUser == null)
                throw new UserNotFoundException($"User with github ID '{githubId}' not found");

            return MapUser(dbUser);
        }

        public async Task<User> GetUserById(Guid id)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
            if (dbUser == null)
                throw new UserNotFoundException($"User '{id}' not found");

            return MapUser(dbUser);
        }

        private static TUser MapUser<TUser>(DbUser dbUser) where TUser : User, new() => new()
        {
            Id = dbUser.Id,
            Created = dbUser.Created,
            Updated = dbUser.Updated,
            GithubId = dbUser.GithubId,
            Email = dbUser.Email,
            UserName = dbUser.UserName,
            GameCount = dbUser.GameCount,
            SubmitCount = dbUser.SubmitCount,
            IsCompiled = dbUser.IsCompiled,
            IsCompiling = dbUser.IsCompiling,
            CompilationErrors = dbUser.CompilationErrors,
            BotLanguage = dbUser.BotLanguage,
            Points = dbUser.Points,
        };

        private static User MapUser(DbUser dbUser) => MapUser<User>(dbUser);

        public async Task AddUser(int githubId, string email, string username)
        {
            this._dbContext.Users.Add(new DbUser
            {
                GithubId = githubId,
                Email = email,
                UserName = username ?? string.Empty,
                GameCount = 0,
                SubmitCount = 0,
                IsCompiled = false,
                IsCompiling = false,
                CompilationErrors = string.Empty,
                BotLanguage = string.Empty,
                Points = Constants.InitialPoints
            });

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateUser(User changedUser)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(e => e.Id == changedUser.Id).ConfigureAwait(false);
            if (dbUser == null)
                throw new UserNotFoundException($"User '{changedUser.Email ?? changedUser.Id.ToString("D")}' not found");

            if (!string.IsNullOrWhiteSpace(changedUser.UserName))
                dbUser.UserName = changedUser.UserName;

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

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<ICollection<RankedUser>> GetRankedUsers()
        {
            return await this._dbContext.Users
                .OrderByDescending(u => u.Points)
                .ThenBy(u => u.Created)
                .Select(u => MapRankedUser(u))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        private static RankedUser MapRankedUser(DbUser u) => new RankedUser
        {
            Id = u.Id,
            UserName = u.UserName,
            BotLanguage = u.BotLanguage,
            Points = u.Points
        };

        public async Task<QueuedTask> GetTask(Guid taskId)
        {
            var dbTask = await this._dbContext.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync().ConfigureAwait(false);
            if (dbTask == null)
                throw new QueuedTaskNotFoundException($"Task '{taskId}' not found");

            return MapQueuedTask(dbTask);
        }

        public Task AddCompilationTask(Guid userId)
        {
            var data = userId.ToString("D");
            return this.AddTask(QueuedTaskType.Compile, data, userId);
        }

        public Task AddGameTask(ICollection<User> users)
        {
            Debug.Assert(users != null);
            Debug.Assert(users.Count == 4);

            // <userGuid>:<userName>,<userGuid:userName>,<userGuid:userName>,<userGuid:userName>
            var data = string.Join(",", users.Select(u => $"{u.Id:D}:{u.UserName}"));
            return this.AddTask(QueuedTaskType.Game, data, null);
        }

        private async Task AddTask(QueuedTaskType type, string data, Guid? relatedUserId)
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
                throw new QueuedTaskNotFoundException("There are no more queued tasks");

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
                throw new QueuedTaskNotFoundException($"The queued task '{taskId}' not found");

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
                throw new QueuedTaskNotFoundException($"The queued task '{taskId}' not found");

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

        public async Task<QueuedTask> GetUserActiveCompileTask(Guid userId)
        {
            var userIdString = userId.ToString("D");

            var dbTask = await this._dbContext.Tasks
                .Where(t => t.Type == QueuedTaskType.Compile)
                .Where(t => t.Status != QueuedTaskStatus.Finished)
                .Where(t => t.Data == userIdString)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return dbTask == null ? null : MapQueuedTask(dbTask);
        }

        public async Task<PaginationModel<GameInfo>> GetPagedUserGames(Guid userId, int page)
        {
            var totalGamesCount = await this._dbContext.GameUsers.CountAsync(gu => gu.UserId == userId).ConfigureAwait(false);

            var pageIndex = page - 1;
            var skipCount = pageIndex * UserGamesPageItemsCount;

            var rows = await this._dbContext.GameUsers
                .Where(gameUser => gameUser.UserId == userId)
                .Join(
                    this._dbContext.Games,
                    gameUser => gameUser.GameId,
                    game => game.Id,
                    (gameUser, game) => new
                    {
                        GameId = game.Id,
                        GameCreated = game.Created
                    }
                )
                .OrderByDescending(x => x.GameCreated)
                .Skip(skipCount)
                .Take(UserGamesPageItemsCount)
                .Join(
                    this._dbContext.GameUsers,
                    tmp => tmp.GameId,
                    gameUser => gameUser.GameId,
                    (tmp, gameUser) => new
                    {
                        GameId = tmp.GameId,
                        GameCreated = tmp.GameCreated,
                        UserId = gameUser.UserId,
                        UserDeltaPoints = gameUser.DeltaPoints,
                        UserRank = gameUser.Rank
                    }
                )
                .Join(
                    this._dbContext.Users,
                    tmp => tmp.UserId,
                    user => user.Id,
                    (tmp, user) => new
                    {
                        GameId = tmp.GameId,
                        GameCreated = tmp.GameCreated,
                        UserId = tmp.UserId,
                        UserDeltaPoints = tmp.UserDeltaPoints,
                        UserRank = tmp.UserRank,
                        UserName = user.UserName,
                        UserGithubId = user.GithubId
                    }
                )
                .OrderByDescending(x => x.GameCreated)
                .ThenBy(x => x.UserRank)
                .ToListAsync()
                .ConfigureAwait(false);

            var games = rows.Aggregate(new Dictionary<Guid, GameInfo>(), (acc, row) =>
            {
                if (!acc.TryGetValue(row.GameId, out var gameInfo))
                {
                    gameInfo = acc[row.GameId] = new GameInfo
                    {
                        Id = row.GameId,
                        Created = row.GameCreated,
                        Users = new List<GameUserInfo>()
                    };
                }

                gameInfo.Users.Add(new GameUserInfo
                {
                    Id = row.UserId,
                    GithubId = row.UserGithubId,
                    Name = row.UserName,
                    DeltaPoints = row.UserDeltaPoints,
                    Rank = row.UserRank
                });

                return acc;
            }).Values;

            return new PaginationModel<GameInfo>(games, page, UserGamesPageItemsCount, totalGamesCount);
        }

        public async Task<Guid> AddGame(GameSummary gameSummary)
        {
            var dbGame = new DbGame();

            dbGame.Errors = gameSummary.Errors;
            dbGame.InitDuration = gameSummary.InitDuration;
            dbGame.GameDuration = gameSummary.GameDuration;
            dbGame.Stdout = gameSummary.StandardOutput;
            dbGame.Stderr = gameSummary.StandardError;

            this._dbContext.Games.Add(dbGame);

            foreach (var (_, playerSummary) in gameSummary.Players)
            {
                var userDbId = playerSummary.WebsiteId!.Value;

                var dbGameUser = new DbGameUser
                {
                    Game = dbGame,
                    UserId = userDbId,
                    Score = playerSummary.Score,
                    DeltaPoints = playerSummary.Points,
                    Rank = playerSummary.Rank,
                    Errors = playerSummary.Errors,
                };

                this._dbContext.GameUsers.Add(dbGameUser);

                var dbUser = await this._dbContext.Users.SingleAsync(u => u.Id == userDbId).ConfigureAwait(false);
                dbUser.Points = playerSummary.Points;
            }

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dbGame.Id;
        }
    }
}