namespace Sentyll.Domain.Common.Abstractions.Failures;

public static class CronScheduleFailures
{
    private const string Code = "CRONT";
    
    public static readonly Failure CannotParse = new(Code, "0001", "Cannot parse expression.");
    public static readonly Failure ExpressionNull = new(Code, "0001", "Cron Expression cannot be null.");
}