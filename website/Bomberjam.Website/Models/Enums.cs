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
        Created = 0,
        Pulled = 1,
        Started = 2,
        Finished = 3
    }

    public enum CompilationStatus
    {
        Unknown = 0,
        NotCompiled = 1,
        CompilationSucceeded = 2,
        CompilationFailed = 3
    }

    public enum ModelType
    {
        Unknown = 0,
        User = 1,
        Bot = 2,
        Game = 3,
        Task = 4
    }
}