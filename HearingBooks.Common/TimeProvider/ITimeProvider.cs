using System;

namespace HearingBooks.Api.Core.TimeProvider;

public interface ITimeProvider
{
    DateTimeOffset UtcNow();
}