using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bomberjam.Common;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Database
{
    public interface IRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserByGithubId(int githubId);
        Task<User> GetUserById(Guid id);
        Task AddUser(int githubId, string email, string username);
        Task UpdateUser(User changedUser);
        Task<ICollection<RankedUser>> GetRankedUsers();

        Task<IEnumerable<Bot>> GetBots(Guid userId);
        Task<Bot> GetBot(Guid botId);
        Task<Guid> AddBot(Guid userId);
        Task UpdateBot(Bot bot);

        Task<QueuedTask> PopNextTask();
        Task<QueuedTask> GetTask(Guid taskId);
        Task AddCompilationTask(Guid botId);
        Task AddGameTask(IReadOnlyCollection<User> users);
        Task MarkTaskAsStarted(Guid taskId);
        Task MarkTaskAsFinished(Guid taskId);

        Task<GameInfo> GetGame(Guid gameId);
        Task<PaginationModel<GameInfo>> GetPagedUserGames(Guid userId, int page);
        Task<Guid> AddGame(GameSummary gameSummary);
    }
}