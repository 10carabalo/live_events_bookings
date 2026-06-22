using System.Net;
using System.Net.Http.Json;
using EventosVivos.Application.Venues;
using FluentAssertions;

namespace EventosVivos.Api.Tests;

public sealed class VenuesEndpointTests(ApiWebApplicationFactory factory)
    : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetVenues_Returns200WithThreeSeededVenues()
    {
        var response = await _client.GetAsync("/api/venues");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var venues = await response.Content.ReadFromJsonAsync<List<VenueDto>>();
        venues.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetVenues_SeedDataValuesAreCorrect()
    {
        var venues = await _client.GetFromJsonAsync<List<VenueDto>>("/api/venues");

        venues.Should().ContainSingle(v => v.Name == "Auditorio Central" && v.Capacity == 200 && v.City == "Bogotá");
        venues.Should().ContainSingle(v => v.Name == "Sala Norte" && v.Capacity == 50 && v.City == "Bogotá");
        venues.Should().ContainSingle(v => v.Name == "Arena Sur" && v.Capacity == 500 && v.City == "Medellín");
    }

    [Fact]
    public async Task UnknownRoute_Returns404ProblemDetails()
    {
        var response = await _client.GetAsync("/api/does-not-exist");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task OpenApiDocument_Returns200()
    {
        var response = await _client.GetAsync("/openapi/v1.json");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Contain("json");
    }
}
