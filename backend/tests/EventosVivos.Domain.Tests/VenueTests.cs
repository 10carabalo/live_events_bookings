using EventosVivos.Domain.Entities;
using FluentAssertions;

namespace EventosVivos.Domain.Tests;

public class VenueTests
{
    [Fact]
    public void Venue_HasExpectedProperties()
    {
        var venue = new Venue(1, "Auditorio Central", 200, "Bogotá");

        venue.Id.Should().Be(1);
        venue.Name.Should().Be("Auditorio Central");
        venue.Capacity.Should().Be(200);
        venue.City.Should().Be("Bogotá");
    }

    [Theory]
    [InlineData(1, "Auditorio Central", 200, "Bogotá")]
    [InlineData(2, "Sala Norte", 50, "Bogotá")]
    [InlineData(3, "Arena Sur", 500, "Medellín")]
    public void Venue_SeedValues_AreCorrect(int id, string name, int capacity, string city)
    {
        var venue = new Venue(id, name, capacity, city);

        venue.Id.Should().Be(id);
        venue.Name.Should().Be(name);
        venue.Capacity.Should().Be(capacity);
        venue.City.Should().Be(city);
    }
}
