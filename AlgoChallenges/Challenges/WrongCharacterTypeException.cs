using System.Runtime.Serialization;

namespace Challenges
{
    [Serializable]
    public class WrongCharacterTypeException : Exception
    {
        private int i;
        private string? v1;
        private char v2;

        public WrongCharacterTypeException()
        {
        }

        public WrongCharacterTypeException(string? message) : base(message)
        {
        }

        public WrongCharacterTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public WrongCharacterTypeException(int i, string v1, char v2)
        {
            this.i = i;
            this.v1 = v1;
            this.v2 = v2;
        }

        protected WrongCharacterTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}