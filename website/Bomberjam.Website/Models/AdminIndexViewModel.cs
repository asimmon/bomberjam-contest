using System.Collections.Generic;
using System.Linq;

namespace Bomberjam.Website.Models
{
    public class AdminIndexViewModel
    {
        public AdminIndexViewModel(IEnumerable<Worker> workers, IEnumerable<User> users, string errorMessage, string successMessage)
        {
            this.Workers = workers.ToList();
            this.Users = users.ToList();
            this.ErrorMessage = errorMessage;
            this.SuccessMessage = successMessage;
        }

        public IReadOnlyList<Worker> Workers { get; }

        public IReadOnlyList<User> Users { get; }

        public string ErrorMessage { get; }

        public string SuccessMessage { get; }
    }
}