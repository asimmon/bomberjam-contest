using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomberjam.Website.Models
{
    public class GameInfo
    {
        public GameInfo(Guid id, DateTime created, GameOrigin origin, IEnumerable<GameUserInfo> users)
        {
            this.Id = id;
            this.Created = created;
            this.Origin = origin;
            this.Users = users.ToList();
        }

        public Guid Id { get; }
        public DateTime Created { get; }
        public GameOrigin Origin { get; }
        public IReadOnlyCollection<GameUserInfo> Users { get; }
    }

    public class GameUserInfo
    {
        public Guid Id { get; set; }
        public string GithubId { get; set; }
        public string UserName { get; set; }
        public float DeltaPoints { get; set;  }
        public int Rank { get; set;  }
        public string Errors { get; set; }
    }
}