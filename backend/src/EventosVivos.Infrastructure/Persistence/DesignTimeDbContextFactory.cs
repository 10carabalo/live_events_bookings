using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EventosVivos.Infrastructure.Persistence;

// Used only by dotnet-ef CLI tooling at design time (migrations add/update).
internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=eventosvivos.db")
            .Options;
        return new AppDbContext(options);
    }
}
