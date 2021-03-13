using System;
using System.ComponentModel.DataAnnotations;

namespace Bomberjam.Website.Database
{
    public class DbUser : ITimestampable
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public int GithubId { get; set; }

        [MaxLength(32)]
        public string UserName { get; set; }

        [MaxLength(128)]
        public string Email { get; set; }

        public float Points { get; set; }
    }
}