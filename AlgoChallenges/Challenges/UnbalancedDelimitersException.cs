using System.Runtime.Serialization;

namespace Challenges
{
    [Serializable]
    internal class UnbalancedDelimitersException : Exception
    {
        public UnbalancedDelimitersException()
        {
        }

        public UnbalancedDelimitersException(string? message) : base(message)
        {
        }

        public UnbalancedDelimitersException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnbalancedDelimitersException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}