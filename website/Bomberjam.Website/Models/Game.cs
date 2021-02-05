using System;
using System.Collections.Generic;

namespace Bomberjam.Website.Models
{
    public class GameInfo
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public Guid? WinnerId { get; set; }
        public IDictionary<Guid, string> UserNames { get; set; }
        public IDictionary<Guid, int> UserScores { get; set; }
    }
}