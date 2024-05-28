namespace MewingPad.Database.PgSQL.Context;

public interface IDbContextFactory
{
    MewingPadPgSQLDbContext GetDbContext();
}