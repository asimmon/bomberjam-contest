using System.Collections.Generic;
using System.Linq;

namespace Bomberjam.Website.Models
{
    public class AdminIndexViewModel
    {
        public AdminIndexViewModel(IEnumerable<Worker> workers, IEnumerable<Season> seasons, IEnumerable<User> users, string environmentName, string errorMessage, string successMessage)
        {
            this.Workers = workers.ToList();
            this.Seasons = seasons.ToList();
            this.Users = users.ToList();
            this.EnvironmentName = environmentName;
            this.ErrorMessage = errorMessage;
            this.SuccessMessage = successMessage;
        }

        public IReadOnlyList<Worker> Workers { get; }

        public IReadOnlyList<Season> Seasons { get; }

        public IReadOnlyList<User> Users { get; }

        public string EnvironmentName { get; }

        public string ErrorMessage { get; }

        public string SuccessMessage { get; }
    }
}