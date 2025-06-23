using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Contracts.Collections;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Delegates;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Jobs;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Jobs;

internal sealed class JobProviderManager() : IJobProviderManager
{

    public IJobCollection Jobs => JobProviderStore.Jobs;
    
    public IRequestCollection Requests => JobProviderStore.Requests;
    
    public void RegisterJobFunction(
        string functionName,
        string cronExpression,
        SchedulerJobPriority priority,
        AsyncSchedulerJobInvocationDelegate @delegate)
        => JobProviderStore.RegisterJobFunction(functionName, cronExpression, priority, @delegate);

    public void RegisterFunctionRequestType(
        string functionName,
        string requestTypeName,
        Type requestType)
        => JobProviderStore.RegisterFunctionRequestType(functionName, requestTypeName, requestType);
    
    public List<(string jobId, string cronSchedule)> GetJobSchedules()
        => JobProviderStore.Jobs
            .Select((jobFunc) => (jobFunc.Key, jobFunc.Value.cronExpression))
            .ToList();

    public bool JobExists(string functionName)
        => JobProviderStore.Jobs.ContainsKey(functionName);

    public bool JobsExist()
        => JobProviderStore.Jobs.Count != 0;

    public bool TryGetJobFunction(
        string functionName, 
        out (
            string cronExpression,
            SchedulerJobPriority Priority,
            AsyncSchedulerJobInvocationDelegate Delegate
        ) job)
        => JobProviderStore.Jobs.TryGetValue(functionName, out job);

}