using System;

namespace Posterr.Base.Exceptions
{
    [Serializable]
    public class MaxDailyPostsReachedException : BasePosterrException
    {
        public MaxDailyPostsReachedException(string message, Exception innerException) : base(message, innerException) { }
        public MaxDailyPostsReachedException(string message) : base(message) { }
        public MaxDailyPostsReachedException() : base() { }
    }


}
