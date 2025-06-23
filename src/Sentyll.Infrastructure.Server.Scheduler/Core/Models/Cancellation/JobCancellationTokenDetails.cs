using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;

namespace Sentyll.Infrastructure.Server.Scheduler.Core.Models.Cancellation;

internal record JobCancellationTokenDetails(
    string FunctionName,
    SchedulerJobType Type,
    bool IsDue,
    CancellationTokenSource CancellationSource
);