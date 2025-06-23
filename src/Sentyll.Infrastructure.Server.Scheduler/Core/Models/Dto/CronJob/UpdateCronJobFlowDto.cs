using Sentyll.Domain.Data.Abstractions.Entities.Scheduler;

namespace Sentyll.Infrastructure.Server.Scheduler.Core.Models.Dto.CronJob;

public record UpdateCronJobFlowDto(CronJobEntity Job)
{
    public DateTime NextOccurrence { get; set; }
    public List<CronJobOccurrenceEntity> QueuedOccurences { get; set; }
    public bool ForceRestart { get; set; }
}