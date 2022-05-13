using System;

namespace Posterr.Base.Exceptions
{
    [Serializable]
    public class PostNotFoundException : BasePosterrException
    {
        public PostNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        public PostNotFoundException(string message) : base(message) { }
        public PostNotFoundException() : base() { }
    }


}
