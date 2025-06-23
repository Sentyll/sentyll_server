namespace Sentyll.Infrastructure.HealthChecks.NpgSql.Core.Constants;

internal static class NpqSqlConstants
{
    
    public const string DefaultPingSqlQuery = "SELECT 1;";

    public static string JsonNotEqualMessage(string expected, string? actual)
        => $"""
            The expected json response does not match the actual response.
            expected: {expected}
            actual: {actual}
            """;
}