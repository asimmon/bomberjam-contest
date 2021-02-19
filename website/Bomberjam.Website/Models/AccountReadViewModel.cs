using System.Collections.Generic;
using System.Linq;

namespace Bomberjam.Website.Models
{
    public class AccountReadViewModel
    {
        public AccountReadViewModel(User user, IEnumerable<Bot> bots)
        {
            this.User = user;
            this.Bots = bots.ToList();
        }

        public User User { get; }

        public IReadOnlyCollection<Bot> Bots { get; }
    }
}