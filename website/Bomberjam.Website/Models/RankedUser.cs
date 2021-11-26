using System;

namespace Bomberjam.Website.Models
{
    public class RankedUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Organization { get; set; }
        public float Points { get; set; }
        public int GlobalRank { get; set; }
        public string GithubId { get; set; }
        public bool HasCompiledBot { get; set; }
    }
}