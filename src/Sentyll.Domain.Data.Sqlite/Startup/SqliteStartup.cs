using Sentyll.Domain.Data.Abstractions.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Sentyll.Domain.Data.Sqlite.Startup;

public static class SqliteStartup
{

    public static IServiceCollection AddSqlitePersistantStorage(
        this IServiceCollection serviceCollection,
        string sqliteConnection,
        Action<DbContextOptionsBuilder>? configureOptions = null,
        Action<SqliteDbContextOptionsBuilder>? configureSqlServerOptions = null
    ) =>
        serviceCollection.AddDbContext<SentyllContext>(optionsBuilder =>
        {
            configureOptions?.Invoke(optionsBuilder);
            optionsBuilder.UseSqlite(sqliteConnection, sqliteBuilder =>
            {
                sqliteBuilder.MigrationsAssembly("Sentyll.Domain.Data.Sqlite");
                configureSqlServerOptions?.Invoke(sqliteBuilder);
            });
        });
}