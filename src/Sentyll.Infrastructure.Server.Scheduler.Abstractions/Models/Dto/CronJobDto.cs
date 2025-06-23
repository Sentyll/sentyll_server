namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Dto;

public sealed record CronJobDto(
    Guid Id, 
    string? Function, 
    DateTime? CreatedAt, 
    DateTime? UpdatedAt, 
    string? Expression, 
    string RequestType, 
    string Description, 
    int Retries, 
    int[]? RetryIntervals, 
    string InitIdentifier) : BaseJobDto(Id, Function, CreatedAt, UpdatedAt);