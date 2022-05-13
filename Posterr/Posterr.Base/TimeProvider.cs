using System;

namespace Posterr.Base
{
    public abstract class TimeProvider : ITimeProvider
    {
        private static ITimeProvider current = new DefaultTimeProvider();

        public static ITimeProvider Current
        {
            get { return TimeProvider.current; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                TimeProvider.current = value;
            }
        }

        public abstract DateTime UtcNow { get; }

        public abstract DateTime Now { get; }

        public abstract DateTimeOffset OffsetNow { get; }

        public abstract DateTimeOffset OffsetUtcNow { get; }

        public static void ResetToDefault()
        {
            TimeProvider.current = new DefaultTimeProvider();
        }
    }
}
