using System;

namespace Bomberjam.Website.Database
{
    public interface ITimestampable
    {
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}