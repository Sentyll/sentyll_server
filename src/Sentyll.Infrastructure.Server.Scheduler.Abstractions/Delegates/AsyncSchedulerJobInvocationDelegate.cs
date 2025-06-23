using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Models.Context;

namespace Sentyll.Infrastructure.Server.Scheduler.Abstractions.Delegates;

public delegate Task AsyncSchedulerJobInvocationDelegate(
    IServiceProvider serviceProvider,
    SchedulerFunctionContext context,
    CancellationToken cancellationToken
);