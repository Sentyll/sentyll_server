using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Dto;

public sealed record TimerJobDto(
    Guid Id, 
    string? Function, 
    DateTime? CreatedAt, 
    DateTime? UpdatedAt,
    string? Description,
    SchedulerJobStatus Status,
    string? LockHolder,
    DateTime? ExecutionTime,
    DateTime? LockedAt,
    DateTime? ExecutedAt,
    string RequestType,
    string? Exception,
    long ElapsedTime,
    int Retries,
    int RetryCount,
    int[]? RetryIntervals,
    string InitIdentifier
    ) : BaseJobDto(Id, Function, CreatedAt, UpdatedAt);