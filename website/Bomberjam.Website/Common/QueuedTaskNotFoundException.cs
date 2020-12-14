using System;

namespace Bomberjam.Website.Common
{
    [Serializable]
    public class QueuedTaskNotFoundException : BomberjamException
    {
        public QueuedTaskNotFoundException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class GameNotFoundException : BomberjamException
    {
        public GameNotFoundException(string message) : base(message)
        {
        }
    }
}