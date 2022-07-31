using System.Runtime.Serialization;

namespace Challenges
{
    [Serializable]
    public class WrongClosingDelimiterCharacterException : Exception
    {
        private int _currentIndex;
        private string? _context;
        private char _expected;
        private char _found;

        public WrongClosingDelimiterCharacterException()
        {
        }

        public WrongClosingDelimiterCharacterException(string? message) : base(message)
        {
        }

        public WrongClosingDelimiterCharacterException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public WrongClosingDelimiterCharacterException(int currentIndex, string? context, char expected, char found)
        {
            this._currentIndex = currentIndex;
            this._context = context;
            this._expected = expected;
            this._found = found;
        }

        protected WrongClosingDelimiterCharacterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}