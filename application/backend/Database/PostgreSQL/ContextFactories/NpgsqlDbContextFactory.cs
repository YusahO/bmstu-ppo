using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MewingPad.Database.PgSQL.Context;

public class NpgsqlDbContextFactory(IConfiguration config) : IDbContextFactory
{
    private readonly IConfiguration _config = config;

    public MewingPadPgSQLDbContext GetDbContext()
    {
        var connName = _config["DbConnection"]!;

        var builder = new DbContextOptionsBuilder<MewingPadPgSQLDbContext>();
        builder.UseNpgsql(_config.GetConnectionString(connName))
            .EnableSensitiveDataLogging();

        return new(builder.Options);
    }
}