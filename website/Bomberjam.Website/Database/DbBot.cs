using System;
using System.ComponentModel.DataAnnotations;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Database
{
    public class DbBot : ITimestampable
    {
        public Guid Id { get; set; }

        public int Iteration { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public Guid UserId { get; set; }

        public DbUser User { get; set; }

        public CompilationStatus Status { get; set; }

        [MaxLength(16)]
        public string Language { get; set; }

        public string Errors { get; set; }
    }
}