using System.Collections.Concurrent;
using Sentyll.Domain.Common.Abstractions.Enums.Scheduler;
using Sentyll.Infrastructure.Server.Scheduler.Core.Models.Cancellation;

namespace Sentyll.Infrastructure.Server.Scheduler.Services.Jobs;

internal static class JobCancellationTokenManager
{
    
    private static readonly ConcurrentDictionary<Guid, JobCancellationTokenDetails>  JobCancellationTokensDictionary = new();
    
    public static IReadOnlyDictionary<Guid, JobCancellationTokenDetails> JobCancellationTokens => JobCancellationTokensDictionary;

    public static bool TryAddJobCancellationToken(
        CancellationTokenSource cancellationSource, 
        string functionName, 
        Guid jobId, 
        SchedulerJobType type, 
        bool isDue) 
        => JobCancellationTokensDictionary.TryAdd(jobId, new JobCancellationTokenDetails(functionName, type, isDue, cancellationSource));

    public static bool TryRemoveJobCancellationToken(Guid jobId) 
        => JobCancellationTokensDictionary.TryRemove(jobId, out _);

    public static void ClearJobCancellationTokens() 
        => JobCancellationTokensDictionary.Clear();

    public static bool TryCancelJobCancellationToken(Guid jobId)
    {
        if (JobCancellationTokensDictionary.TryRemove(jobId, out var jobCancellationToken))
        {
            jobCancellationToken.CancellationSource.Cancel();
            return true;
        }
        
        return false;
    }
}