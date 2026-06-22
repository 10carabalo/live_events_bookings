using EventosVivos.Domain.Entities;

namespace EventosVivos.Domain.Repositories;

public interface IVenueRepository
{
    Task<IReadOnlyList<Venue>> GetAllAsync(CancellationToken cancellationToken = default);
}
