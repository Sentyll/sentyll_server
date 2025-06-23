using Microsoft.Extensions.DependencyInjection;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Jobs;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Services.Jobs;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Extensions;

public static class JobRegistrationExtensions
{
    public static IServiceProvider ConfigureJob<TJob>(this IServiceProvider serviceProvider) where TJob : Job
    {
        var job = ActivatorUtilities.CreateInstance<TJob>(serviceProvider);
        var jobStoreManager = serviceProvider.GetRequiredService<IJobProviderManager>();
        
        jobStoreManager.RegisterJobFunction(job.JobIdentifier, job.CronSchedule, job.Priority, job.InvokeFunc);
        
        if (job.RequestType != null)
        {
            var (jobId, requestObj) = job.RequestType.Value;
            
            jobStoreManager.RegisterFunctionRequestType(
                jobId,
                requestObj.FullName ?? throw new ArgumentException("Job Type FullName cannot be null"),
                requestObj
            );
        }
        
        return serviceProvider;
    }
}