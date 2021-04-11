using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bomberjam.Website.Database
{
    public class DbSeason : ITimestampable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? Finished { get; set; }
        public int? UserCount { get; set; }
    }
}