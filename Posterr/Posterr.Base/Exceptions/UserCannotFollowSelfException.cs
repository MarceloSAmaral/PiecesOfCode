using System;

namespace Posterr.Base.Exceptions
{
    [Serializable]
    public class UserCannotFollowSelfException : BasePosterrException
    {
        public UserCannotFollowSelfException(string message, Exception innerException) : base(message, innerException) { }
        public UserCannotFollowSelfException(string message) : base(message) { }
        public UserCannotFollowSelfException() : base() { }
    }


}
