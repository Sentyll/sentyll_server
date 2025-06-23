namespace Sentyll.Infrastructure.HealthChecks.MongoDb.Core.Constants;

internal static class MdbConstants
{
    
    // When running the tests locally during development, don't re-attempt
    // as it prolongs the time it takes to run the tests.
    public const int MaxPingAttempts
#if DEBUG
        = 1;
#else
        = 2;
#endif

    public const string DefaultPingCommand = "{ping:1}";

}