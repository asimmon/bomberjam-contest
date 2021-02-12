using System.Collections.Generic;

namespace Bomberjam.Website.Models
{
    public class UserDetails : User
    {
        public UserDetails(User user, IReadOnlyCollection<GameInfo> games)
        {
            this.User = user;
            this.Games = games;
        }

        public User User { get; }

        public IReadOnlyCollection<GameInfo> Games { get; }
    }
}