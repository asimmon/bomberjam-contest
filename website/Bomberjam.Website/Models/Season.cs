using System;

namespace Bomberjam.Website.Models
{
    public class Season
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Title { get; set; }
        public int? UserCount { get; set; }
    }
}