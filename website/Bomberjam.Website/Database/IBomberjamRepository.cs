using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bomberjam.Common;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Database
{
    public interface IBomberjamRepository
    {
        Task<ITransaction> CreateTransaction();

        Task<IEnumerable<User>> GetUsersWithCompiledBot();
        Task<User> GetUserByGithubId(string githubId);
        Task<User> GetUserById(Guid id);
        Task<User> AddUser(string githubId, string username);
        Task UpdateUser(User changedUser);
        Task<ICollection<RankedUser>> GetRankedUsers();
        Task<IEnumerable<User>> GetAllUsers();
        Task<bool> IsUserNameAlreadyUsed(string username);
        Task UpdateAllUserGlobalRanks(int seasonId);
        Task UpdateAllUserGlobalRanks();

        Task<int> GetBotsCount(Guid userId);
        Task<IEnumerable<Bot>> GetBots(Guid userId, int? max = null);
        Task<Bot> GetBot(Guid botId);
        Task<Guid> AddBot(Guid userId);
        Task UpdateBot(Bot bot);

        Task<QueuedTask> PopNextTask();
        Task<QueuedTask> GetTask(Guid taskId);
        Task<bool> HasPendingGameTasks();
        Task AddCompilationTask(Guid botId);
        Task AddGameTask(IReadOnlyCollection<User> users, GameOrigin origin);
        Task MarkTaskAsCreated(Guid taskId);
        Task MarkTaskAsStarted(Guid taskId);
        Task MarkTaskAsFinished(Guid taskId);
        Task<IEnumerable<QueuedTask>> GetOrphanedTasks();

        Task<GameInfo> GetGame(Guid gameId);
        Task<PaginationModel<GameInfo>> GetPagedUserGames(Guid userId, int seasonId, int page);
        Task<Guid> AddGame(GameSummary gameSummary, GameOrigin origin, int seasonId);

        Task<IEnumerable<Worker>> GetWorkers(int max);
        Task<Worker> AddOrUpdateWorker(Guid id);

        Task<Season> GetCurrentSeason();
        Task<Season> GetSeason(int id);
        Task<IEnumerable<Season>> GetSeasons();
        Task StartNewSeason();
        Task<IEnumerable<SeasonSummary>> GetSeasonSummaries(Guid userId);
    }
}