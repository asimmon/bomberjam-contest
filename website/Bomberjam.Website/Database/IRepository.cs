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
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(Guid id);
        Task AddUser(string email, string username);
        Task UpdateUser(User changedUser);

        Task<QueuedTask> PopNextTask();
        Task<QueuedTask> GetTask(Guid taskId);
        Task AddCompilationTask(Guid userId);
        Task AddGameTask(ICollection<User> users);
        Task MarkTaskAsStarted(Guid taskId);
        Task MarkTaskAsFinished(Guid taskId);
        Task<QueuedTask> GetUserActiveCompileTask(Guid userId);

        Task<IEnumerable<Game>> GetGames();
        Task<Guid> AddGame(GameSummary gameSummary);
        Task<Game> GetGame(Guid id);
    }
}