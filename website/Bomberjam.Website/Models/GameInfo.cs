using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomberjam.Website.Models
{
    public class GameInfo
    {
        public GameInfo(Guid id, DateTime created, IEnumerable<GameUserInfo> users)
        {
            this.Id = id;
            this.Created = created;
            this.Users = users.ToList();
        }

        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public IReadOnlyCollection<GameUserInfo> Users { get; set; }
    }

    public class GameUserInfo
    {
        public Guid Id { get; set; }
        public int GithubId { get; set; }
        public string UserName { get; set; }
        public float DeltaPoints { get; set;  }
        public int Rank { get; set;  }
    }
}