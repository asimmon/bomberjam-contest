using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bomberjam.Common;
using Bomberjam.Website.Models;
using Bomberjam.Website.Utils;

namespace Bomberjam.Website.Database
{
    public class CachedDatabaseRepository : IBomberjamRepository
    {
        private const string GetCurrentSeasonKeyFormat = "GetCurrentSeason";
        private const string GetSeasonsKeyFormat = "GetSeasons";
        private const string GetRankedUsersKeyFormat = "GetRankedUsers";
        private const string GetGameKeyFormat = "GetGame_{0}";

        private readonly IBomberjamRepository _underlyingRepository;
        private readonly IObjectCache _objectCache;

        public CachedDatabaseRepository(IBomberjamRepository underlyingRepository, IObjectCache objectCache)
        {
            this._underlyingRepository = underlyingRepository;
            this._objectCache = objectCache;
        }

        public Task<ITransaction> CreateTransaction()
        {
            return this._underlyingRepository.CreateTransaction();
        }

        public Task<IEnumerable<User>> GetUsersWithCompiledBot()
        {
            return this._underlyingRepository.GetUsersWithCompiledBot();
        }

        public Task<User> GetUserByGithubId(string githubId)
        {
            return this._underlyingRepository.GetUserByGithubId(githubId);
        }

        public Task<User> GetUserById(Guid id)
        {
            return this._underlyingRepository.GetUserById(id);
        }

        public Task<User> AddUser(string githubId, string username)
        {
            this._objectCache.Remove(GetRankedUsersKeyFormat);
            return this._underlyingRepository.AddUser(githubId, username);
        }

        public Task UpdateUser(User changedUser)
        {
            this._objectCache.Remove(GetRankedUsersKeyFormat);
            return this._underlyingRepository.UpdateUser(changedUser);
        }

        public Task<ICollection<RankedUser>> GetRankedUsers()
        {
            return this._objectCache.GetOrSetAsync(GetRankedUsersKeyFormat, () =>
            {
                return this._underlyingRepository.GetRankedUsers();
            });
        }

        public Task<IEnumerable<User>> GetAllUsers()
        {
            return this._underlyingRepository.GetAllUsers();
        }

        public Task<bool> IsUserNameAlreadyUsed(string username)
        {
            return this._underlyingRepository.IsUserNameAlreadyUsed(username);
        }

        public Task UpdateAllUserGlobalRanks(int seasonId)
        {
            this._objectCache.Remove(GetRankedUsersKeyFormat);
            return this._underlyingRepository.UpdateAllUserGlobalRanks(seasonId);
        }

        public Task UpdateAllUserGlobalRanks()
        {
            this._objectCache.Remove(GetRankedUsersKeyFormat);
            return this._underlyingRepository.UpdateAllUserGlobalRanks();
        }

        public Task<IEnumerable<Bot>> GetBots(Guid userId, int? max = null)
        {
            return this._underlyingRepository.GetBots(userId, max);
        }

        public Task<Bot> GetBot(Guid botId)
        {
            return this._underlyingRepository.GetBot(botId);
        }

        public Task<Guid> AddBot(Guid userId)
        {
            return this._underlyingRepository.AddBot(userId);
        }

        public Task UpdateBot(Bot bot)
        {
            return this._underlyingRepository.UpdateBot(bot);
        }

        public Task<QueuedTask> PopNextTask()
        {
            return this._underlyingRepository.PopNextTask();
        }

        public Task<QueuedTask> GetTask(Guid taskId)
        {
            return this._underlyingRepository.GetTask(taskId);
        }

        public Task<bool> HasPendingGameTasks()
        {
            return this._underlyingRepository.HasPendingGameTasks();
        }

        public Task AddCompilationTask(Guid botId)
        {
            return this._underlyingRepository.AddCompilationTask(botId);
        }

        public Task AddGameTask(IReadOnlyCollection<User> users, GameOrigin origin)
        {
            return this._underlyingRepository.AddGameTask(users, origin);
        }

        public Task MarkTaskAsCreated(Guid taskId)
        {
            return this._underlyingRepository.MarkTaskAsCreated(taskId);
        }

        public Task MarkTaskAsStarted(Guid taskId)
        {
            return this._underlyingRepository.MarkTaskAsStarted(taskId);
        }

        public Task MarkTaskAsFinished(Guid taskId)
        {
            return this._underlyingRepository.MarkTaskAsFinished(taskId);
        }

        public Task<IEnumerable<QueuedTask>> GetOrphanedTasks()
        {
            return this._underlyingRepository.GetOrphanedTasks();
        }

        public Task<GameInfo> GetGame(Guid gameId)
        {
            return this._objectCache.GetOrSetAsync(string.Format(GetGameKeyFormat, gameId), () =>
            {
                return this._underlyingRepository.GetGame(gameId);
            });
        }

        public Task<PaginationModel<GameInfo>> GetPagedUserGames(Guid userId, int seasonId, int page)
        {
            return this._underlyingRepository.GetPagedUserGames(userId, seasonId, page);
        }

        public Task<Guid> AddGame(GameSummary gameSummary, GameOrigin origin, int seasonId)
        {
            this._objectCache.Remove(GetRankedUsersKeyFormat);
            return this._underlyingRepository.AddGame(gameSummary, origin, seasonId);
        }

        public Task<IEnumerable<Worker>> GetWorkers(int max)
        {
            return this._underlyingRepository.GetWorkers(max);
        }

        public Task<Worker> AddOrUpdateWorker(Guid id)
        {
            return this._underlyingRepository.AddOrUpdateWorker(id);
        }

        public Task<Season> GetCurrentSeason()
        {
            return this._objectCache.GetOrSetAsync(GetCurrentSeasonKeyFormat, () =>
            {
                return this._underlyingRepository.GetCurrentSeason();
            });
        }

        public Task<Season> GetSeason(int id)
        {
            return this._underlyingRepository.GetSeason(id);
        }

        public Task<IEnumerable<Season>> GetSeasons()
        {
            return this._objectCache.GetOrSetAsync(GetSeasonsKeyFormat, () =>
            {
                return this._underlyingRepository.GetSeasons();
            });
        }

        public Task StartNewSeason()
        {
            this._objectCache.Remove(GetCurrentSeasonKeyFormat);
            this._objectCache.Remove(GetSeasonsKeyFormat);

            return this._underlyingRepository.StartNewSeason();
        }

        public Task<IEnumerable<SeasonSummary>> GetSeasonSummaries(Guid userId)
        {
            return this._underlyingRepository.GetSeasonSummaries(userId);
        }
    }
}