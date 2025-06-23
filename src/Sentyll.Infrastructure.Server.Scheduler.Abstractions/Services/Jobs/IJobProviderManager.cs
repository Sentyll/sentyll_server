using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Contracts.Collections;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Delegates;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Jobs;

public interface IJobProviderManager
{

    public IJobCollection Jobs { get; }
    
    public IRequestCollection Requests { get; }
    
    public void RegisterJobFunction(
        string functionName,
        string cronExpression,
        SchedulerJobPriority priority,
        AsyncSchedulerJobInvocationDelegate @delegate
    );

    public void RegisterFunctionRequestType(
        string functionName,
        string requestTypeName,
        Type requestType
    );
    
    List<(string jobId, string cronSchedule)> GetJobSchedules();

    bool JobExists(string functionName);

    bool JobsExist();

    bool TryGetJobFunction(
        string functionName,
        out (
            string cronExpression,
            SchedulerJobPriority Priority,
            AsyncSchedulerJobInvocationDelegate Delegate
            ) job
    );

}