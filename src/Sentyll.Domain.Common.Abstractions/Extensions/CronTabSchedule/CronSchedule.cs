using NCrontab;
using Sentyll.Domain.Common.Abstractions.Failures;

namespace Sentyll.Domain.Common.Abstractions.Extensions.CronTabSchedule;

public static class CronSchedule
{

    public static bool IsValidCronTabSchedule(this string expression)
        => CrontabSchedule.TryParse(expression) != null;
    
    public static Result<CrontabSchedule> Parse(string? expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return Result.Failure<CrontabSchedule>(CronScheduleFailures.ExpressionNull);
        }
        
        return CrontabSchedule.Parse(expression);
    }

    public static Result<CrontabSchedule> TryParse(string? expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return Result.Failure<CrontabSchedule>(CronScheduleFailures.ExpressionNull);
        }
        
        if (!(CrontabSchedule.TryParse(expression) is { } crontabSchedule))
        {
            return Result.Failure<CrontabSchedule>(CronScheduleFailures.CannotParse);
        }

        return crontabSchedule;
    }

    public static Result<DateTime> GetNextOccurrence(string? expression, DateTime utcNow)
        => Parse(expression)
            .Map((schedule) => schedule.GetNextOccurrence(utcNow));
    
    public static Result<DateTime> TryGetNextOccurrence(string? expression, DateTime utcNow)
        => TryParse(expression)
            .Map((schedule) => schedule.GetNextOccurrence(utcNow));
    
    public static Result<IEnumerable<DateTime>> GetNextOccurrences(string? expression, DateTime fromUtcNow, DateTime toUtcTime)
        => Parse(expression)
            .Map((schedule) => schedule.GetNextOccurrences(fromUtcNow, toUtcTime));
    
    public static Result<IEnumerable<DateTime>> TryGetNextOccurrences(string? expression, DateTime fromUtcNow, DateTime toUtcTime)
        => TryParse(expression)
            .Map((schedule) => schedule.GetNextOccurrences(fromUtcNow, toUtcTime));

}