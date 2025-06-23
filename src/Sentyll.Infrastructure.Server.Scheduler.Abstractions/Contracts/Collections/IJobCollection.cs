using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Delegates;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Contracts.Collections;

public interface IJobCollection : IReadOnlyDictionary<
    string, 
    (
        string cronExpression, 
        SchedulerJobPriority Priority, 
        AsyncSchedulerJobInvocationDelegate Delegate
    )
>;