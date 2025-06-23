using Npgsql;

namespace Sentyll.Infrastructure.HealthChecks.NpgSql.Services;

internal sealed class NpqSqlCommandService
{
    public async Task<object?> ExecuteQueryAsync(
        string connectionString, 
        string query,
        CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        await connection
            .OpenAsync(cancellationToken)
            .ConfigureAwait(false);

        await using var command = connection.CreateCommand();
        command.CommandText = query;
        
        return await command
            .ExecuteScalarAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}