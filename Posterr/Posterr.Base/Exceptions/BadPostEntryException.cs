using System;

namespace Posterr.Base.Exceptions
{
    [Serializable]
    public class BadPostEntryException : BasePosterrException
    {
        public BadPostEntryException(string message, Exception innerException) : base(message, innerException) { }
        public BadPostEntryException(string message) : base(message) { }
        public BadPostEntryException() : base() { }
    }


}
