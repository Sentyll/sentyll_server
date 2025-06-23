using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Dto;

public sealed record JobOccurrenceDto(
    Guid Id, 
    SchedulerJobStatus Status, 
    string? LockHolder, 
    DateTime ExecutionTime, 
    Guid CronJobId, 
    DateTime? LockedAt, 
    DateTime? ExecutedAt, 
    string? Exception, 
    long ElapsedTime, 
    int RetryCount
    );