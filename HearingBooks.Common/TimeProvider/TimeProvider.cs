using System;

namespace HearingBooks.Api.Core.TimeProvider;

public class TimeProvider : ITimeProvider
{
    public DateTimeOffset UtcNow()
    {
        return DateTimeOffset.UtcNow;
    }
}