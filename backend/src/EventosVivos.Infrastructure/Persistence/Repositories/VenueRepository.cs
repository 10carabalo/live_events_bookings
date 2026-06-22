using EventosVivos.Domain.Entities;
using EventosVivos.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventosVivos.Infrastructure.Persistence.Repositories;

internal sealed class VenueRepository(AppDbContext context) : IVenueRepository
{
    public async Task<IReadOnlyList<Venue>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Venues
            .OrderBy(v => v.Id)
            .ToListAsync(cancellationToken);
    }
}
