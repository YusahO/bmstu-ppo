using Microsoft.EntityFrameworkCore;

namespace MewingPad.Database.PgSQL.Context;

public class InMemoryDbContextFactory() : IDbContextFactory
{
    private readonly string _dbName = $"MewingPadTestDb_{Guid.NewGuid()}";

    public MewingPadPgSQLDbContext GetDbContext()
    {
        var builder = new DbContextOptionsBuilder<MewingPadPgSQLDbContext>();
        builder.UseInMemoryDatabase(_dbName);

        return new(builder.Options);
    }
}