using Microsoft.Data.SqlClient;

namespace Sentyll.Infrastructure.HealthChecks.SqlServer.Services;

internal sealed class SqlServerConnectionService
{
    public async Task<object?> ExecuteQueryAsync(
        string connectionString,
        string query,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);

        await connection
            .OpenAsync(cancellationToken)
            .ConfigureAwait(false);

        await using var command = connection.CreateCommand();
        command.CommandText = query;
        
        return  await command
            .ExecuteScalarAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}