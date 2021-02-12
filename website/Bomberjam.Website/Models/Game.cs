using System;
using System.Collections.Generic;

namespace Bomberjam.Website.Models
{
    public class GameInfo
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public IList<GameUserInfo> Users { get; set; }
    }

    public class GameUserInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float DeltaPoints { get; set;  }
        public int Rank { get; set;  }
    }
}