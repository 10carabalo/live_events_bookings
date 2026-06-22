using EventosVivos.Application.Venues;
using EventosVivos.Domain.Entities;
using EventosVivos.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace EventosVivos.Application.Tests;

public class ListVenuesTests
{
    private readonly Mock<IVenueRepository> _repoMock = new();

    [Fact]
    public async Task ListVenues_ReturnsAllVenuesMappedToDtos()
    {
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new Venue(1, "Auditorio Central", 200, "Bogotá"),
                new Venue(2, "Sala Norte", 50, "Bogotá"),
                new Venue(3, "Arena Sur", 500, "Medellín")
            ]);

        var useCase = new ListVenuesUseCase(_repoMock.Object);
        var result = await useCase.ExecuteAsync(CancellationToken.None);

        result.Should().HaveCount(3);
        result[0].Id.Should().Be(1);
        result[0].Name.Should().Be("Auditorio Central");
        result[0].Capacity.Should().Be(200);
        result[0].City.Should().Be("Bogotá");
    }

    [Fact]
    public async Task ListVenues_EmptyRepository_ReturnsEmptyList()
    {
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var useCase = new ListVenuesUseCase(_repoMock.Object);
        var result = await useCase.ExecuteAsync(CancellationToken.None);

        result.Should().BeEmpty();
    }
}
