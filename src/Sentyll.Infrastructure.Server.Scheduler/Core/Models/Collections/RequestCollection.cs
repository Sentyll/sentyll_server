using System.Collections.Concurrent;
using Sentyll.Infrastructure.Server.Scheduler.Abstractions.Contracts.Collections;

namespace Sentyll.Infrastructure.Server.Scheduler.Core.Models.Collections;

internal sealed class RequestCollection : ConcurrentDictionary<
    string, 
    (
        string,
        Type
    )
>, IRequestCollection
{
    
}