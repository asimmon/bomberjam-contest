namespace Bomberjam.Website.Models
{
    public enum QueuedTaskType
    {
        Unknown = 0,
        Compile = 1,
        Game = 2
    }

    public enum QueuedTaskStatus
    {
        Created = 1,
        Pulled = 2,
        Started = 3,
        Finished = 4
    }
}