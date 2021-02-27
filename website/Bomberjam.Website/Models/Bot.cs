using System;

namespace Bomberjam.Website.Models
{
    public class Bot
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public CompilationStatus Status { get; set; }
        public string Language { get; set; }
        public string Errors { get; set; }
        public int GameCount { get; set; }
    }
}