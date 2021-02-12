using System;

namespace Bomberjam.Website.Database
{
    public class DbUser : ITimestampable
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int GithubId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int SubmitCount { get; set; }
        public int GameCount { get; set; }
        public bool IsCompiling { get; set; }
        public bool IsCompiled { get; set; }
        public string CompilationErrors { get; set; }
        public string BotLanguage { get; set; }
        public float Points { get; set; }
    }
}