using EventosVivos.Application.Venues;
using Microsoft.AspNetCore.Mvc;

namespace EventosVivos.Api.Controllers;

[ApiController]
[Route("api/venues")]
public sealed class VenuesController(ListVenuesUseCase listVenues) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var venues = await listVenues.ExecuteAsync(cancellationToken);
        return Ok(venues);
    }
}
