using System.Collections.Generic;
using System.Linq;

namespace Bomberjam.Website.Models
{
    public class AdminIndexViewModel
    {
        public AdminIndexViewModel(IEnumerable<Worker> workers, IEnumerable<User> users)
        {
            this.Workers = workers.ToList();
            this.Users = users.ToList();
        }

        public IReadOnlyList<Worker> Workers { get; }

        public IReadOnlyList<User> Users { get; }
    }
}