using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Unknown")]
        Unknown = 0,

        [Display(Name = "Not compiled yet")]
        NotCompiled = 1,

        [Display(Name = "Compilation succeeded")]
        CompilationSucceeded = 2,

        [Display(Name = "Compilation failed")]
        CompilationFailed = 3
    }

    public enum EntityType
    {
        Unknown = 0,
        User = 1,
        Bot = 2,
        Game = 3,
        Task = 4
    }

    public enum GameOrigin
    {
        [Display(Name = "Ranked")]
        RankedMatchmaking = 0,

        [Display(Name = "Practice")]
        TestingPurpose = 1,

        [Display(Name = "On demand")]
        OnDemand = 2,
    }
}