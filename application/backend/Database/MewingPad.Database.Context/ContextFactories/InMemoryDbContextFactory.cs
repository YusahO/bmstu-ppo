using Microsoft.EntityFrameworkCore;

namespace MewingPad.Database.Context;

public class InMemoryDbContextFactory() : IDbContextFactory
{
    private readonly string _dbName = $"MewingPadTestDb_{Guid.NewGuid()}";

    public MewingPadDbContext GetDbContext()
    {
        var builder = new DbContextOptionsBuilder<MewingPadDbContext>();
        builder.UseInMemoryDatabase(_dbName);

        return new(builder.Options);
    }
}