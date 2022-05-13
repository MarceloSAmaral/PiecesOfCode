using System;

namespace Posterr.Base
{
    public class DefaultTimeProvider : TimeProvider
    {
        public override DateTime UtcNow => DateTime.UtcNow;

        public override DateTime Now => DateTime.Now;

        public override DateTimeOffset OffsetNow => DateTimeOffset.Now;

        public override DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;
    }
}
