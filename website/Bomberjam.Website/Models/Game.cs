using System;
using System.Collections.Generic;

namespace Bomberjam.Website.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsFinished { get; set; }
        public Guid? WinnerId { get; set; }
        public string Errors { get; set; }
        public List<GameUser> Users { get; set; }
    }
}