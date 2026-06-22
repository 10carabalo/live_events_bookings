using EventosVivos.Domain.Abstractions;

namespace EventosVivos.Infrastructure.Services;

internal sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
