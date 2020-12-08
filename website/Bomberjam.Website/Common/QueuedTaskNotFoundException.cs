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
}