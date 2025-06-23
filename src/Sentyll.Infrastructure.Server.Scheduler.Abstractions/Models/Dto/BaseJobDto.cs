namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Dto;

public abstract record BaseJobDto(
    Guid Id, 
    string? Function, 
    DateTime? CreatedAt, 
    DateTime? UpdatedAt
    );