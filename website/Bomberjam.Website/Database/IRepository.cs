using System.Collections.Generic;
using System.Threading.Tasks;
using Bomberjam.Website.Controllers;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Database
{
    public interface IRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int id);
        Task AddUser(string email, string username);
        Task UpdateUser(User changedUser);

        Task<QueuedTask> PopNextTask();
        Task<QueuedTask> GetTask(int taskId);
        Task AddCompilationTask(int userId);
        Task AddGameTask(IDictionary<int, string> userIdAndNames);
        Task MarkTaskAsStarted(int taskId);
        Task MarkTaskAsFinished(int taskId);
        Task<bool> DoesUserHaveActiveCompileTask(int userId);

        Task<int> AddGame(ICollection<int> userIds);
        Task<Game> GetGame(int id);
    }
}