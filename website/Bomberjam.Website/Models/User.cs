using System;
using System.Collections.Generic;

namespace Bomberjam.Website.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int GameCount { get; set; }
        public int SubmitCount { get; set; }
        public bool IsCompiled { get; set; }
        public bool IsCompiling { get; set; }
        public string CompilationErrors { get; set; }
        public string BotLanguage { get; set; }

        public bool NeedsToCompleteProfile()
        {
            return string.IsNullOrWhiteSpace(this.UserName);
        }
    }

    public class GameUser : User
    {
        public string Errors { get; set; }
    }

    public class Game
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsFinished { get; set; }
        public int? WinnerId { get; set; }
        public string Errors { get; set; }
        public List<GameUser> Users { get; set; }
    }
}