using System;

namespace Posterr.Base.Exceptions
{
    [Serializable]
    public class BasePosterrException : Exception
    {
        public BasePosterrException(string message, Exception innerException) : base(message, innerException) { }
        public BasePosterrException(string message) : base(message) { }
        public BasePosterrException() : base() { }
    }
}
