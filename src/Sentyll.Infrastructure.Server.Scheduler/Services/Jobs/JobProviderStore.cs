using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Contracts.Collections;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Delegates;
using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Collections;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Jobs;

internal static class JobProviderStore
{

    private static readonly JobCollection JobCollection;
    private static readonly RequestCollection RequestCollection;
    
    public static IJobCollection Jobs => JobCollection;
    public static IRequestCollection Requests => RequestCollection;

    static JobProviderStore()
    {
        JobCollection = new JobCollection();
        RequestCollection = new RequestCollection();
    }

    public static void RegisterJobFunction(
        string functionName,
        string cronExpression,
        SchedulerJobPriority priority,
        AsyncSchedulerJobInvocationDelegate @delegate)
        => JobCollection.TryAdd(functionName, (cronExpression, priority, @delegate));

    public static void RegisterFunctionRequestType(
        string functionName,
        string requestTypeName,
        Type requestType)
        => RequestCollection.TryAdd(functionName, (requestTypeName, requestType));
}