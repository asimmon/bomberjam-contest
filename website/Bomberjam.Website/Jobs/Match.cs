using System.Collections.Generic;
using System.Diagnostics;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Jobs
{
    public class Match
    {
        public IReadOnlyList<User> Users { get; }

        public Match(IReadOnlyList<User> users)
        {
            Debug.Assert(users != null && users.Count == 4);
            this.Users = users;
        }
    }
}