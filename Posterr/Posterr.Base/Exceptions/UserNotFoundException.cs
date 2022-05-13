using System;

namespace Posterr.Base.Exceptions
{
    [Serializable]
    public class UserNotFoundException : BasePosterrException
    {
        public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        public UserNotFoundException(string message) : base(message) { }
        public UserNotFoundException() : base() { }
    }
}
