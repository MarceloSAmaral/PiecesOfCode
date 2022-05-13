using System;

namespace Posterr.Base.Exceptions
{
    [Serializable]
    public class MaxPostSizeExceededException : BasePosterrException
    {
        public MaxPostSizeExceededException(string message, Exception innerException) : base(message, innerException) { }
        public MaxPostSizeExceededException(string message) : base(message) { }
        public MaxPostSizeExceededException() : base() { }
    }
}
