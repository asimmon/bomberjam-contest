using System;

namespace Bomberjam.Website.Models
{
    public class RankedUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string BotLanguage { get; set; }
        public float Points { get; set; }
    }
}