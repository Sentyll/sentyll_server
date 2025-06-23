namespace Sentyll.Infrastructure.HealthChecks.SqlServer.Core.Constants;

internal static class SsConstants
{
    public const string DefaultPingSqlQuery = "SELECT 1;";

    public static string JsonNotEqualMessage(string expected, string? actual)
        => $"""
            The expected json response does not match the actual response.
            expected: {expected}
            actual: {actual}
            """;
}