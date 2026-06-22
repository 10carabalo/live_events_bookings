using EventosVivos.Domain.Repositories;

namespace EventosVivos.Application.Venues;

public sealed class ListVenuesUseCase(IVenueRepository venueRepository)
{
    public async Task<IReadOnlyList<VenueDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var venues = await venueRepository.GetAllAsync(cancellationToken);

        return venues
            .Select(v => new VenueDto(v.Id, v.Name, v.Capacity, v.City))
            .ToList();
    }
}
