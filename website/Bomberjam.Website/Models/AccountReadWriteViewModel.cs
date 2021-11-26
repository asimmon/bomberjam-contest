using System.Collections.Generic;
using System.Linq;

namespace Bomberjam.Website.Models
{
    public class AccountReadWriteViewModel : AccountWriteViewModel
    {
        public AccountReadWriteViewModel(User user, IEnumerable<Bot> bots)
        {
            this.User = user;
            this.Bots = bots.ToList();
            this.UserName = user.UserName;
            this.Organization = user.Organization;
        }

        public AccountReadWriteViewModel(User user, IEnumerable<Bot> bots, AccountWriteViewModel writeViewModel)
            : base(writeViewModel)
        {
            this.User = user;
            this.Bots = bots.ToList();
        }

        public User User { get; }

        public IReadOnlyCollection<Bot> Bots { get; }
    }
}