using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MewingPad.Database.Context;

public class NpgsqlDbContextFactory(IConfiguration config) : IDbContextFactory
{
    private readonly IConfiguration _config = config;

    public MewingPadDbContext GetDbContext()
    {
        var connName = _config["Database Connection"]!;

        var builder = new DbContextOptionsBuilder<MewingPadDbContext>();
        builder.UseNpgsql(_config.GetConnectionString(connName));

        return new(builder.Options);
    }
}