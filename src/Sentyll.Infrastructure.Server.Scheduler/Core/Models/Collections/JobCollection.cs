using System.Collections.Concurrent;
using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Contracts.Collections;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Delegates;

namespace Sentyll.Infrastructure.Server.Scheduler.Core.Models.Collections;

internal sealed class JobCollection : ConcurrentDictionary<
    string, 
    (
        string cronExpression, 
        SchedulerJobPriority Priority, 
        AsyncSchedulerJobInvocationDelegate Delegate
    )
>, IJobCollection
{
    
}