using System;

namespace Bomberjam.Website.Common
{
    [Serializable]
    public class UserNotFoundException : BomberjamException
    {
        public UserNotFoundException(string message)
            : base(message)
        {
        }
    }
}