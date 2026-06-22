namespace EventosVivos.Domain.Abstractions;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
