namespace Sentyll.Domain.Common.Abstractions.Contracts.Queues;

public interface IUnboundedInMemoryQueue<TEventType>
{

    ValueTask PublishAsync(
        TEventType topic,
        CancellationToken cancellationToken = default
    );

    ValueTask<TEventType> ReadAsync(
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<TEventType> ReadAllAsync(
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<TEventType> ContinuallyReadAllAsync(
        CancellationToken cancellationToken = default
    );
    
    bool IsCleared();
    
}