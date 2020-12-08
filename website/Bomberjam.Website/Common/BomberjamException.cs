using System;

namespace Bomberjam.Website.Common
{
    [Serializable]
    public abstract class BomberjamException : Exception
    {
        protected BomberjamException(string message)
            : base(message)
        {
        }

        protected BomberjamException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}