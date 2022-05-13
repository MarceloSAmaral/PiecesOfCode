using System;

namespace Posterr.Base
{
    public interface ITimeProvider
    {
        DateTime UtcNow { get; }

        DateTime Now { get; }

        DateTimeOffset OffsetNow { get; }

        DateTimeOffset OffsetUtcNow { get; }
    }
}
