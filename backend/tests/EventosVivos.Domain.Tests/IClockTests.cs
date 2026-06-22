using EventosVivos.Domain.Abstractions;
using FluentAssertions;

namespace EventosVivos.Domain.Tests;

public class IClockTests
{
    [Fact]
    public void IClock_Contract_ExposesUtcNow()
    {
        // IClock must expose UtcNow — verified by the mock implementation
        var clock = new FakeClock(new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero));

        clock.UtcNow.Should().Be(new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero));
    }

    private sealed class FakeClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow => utcNow;
    }
}
