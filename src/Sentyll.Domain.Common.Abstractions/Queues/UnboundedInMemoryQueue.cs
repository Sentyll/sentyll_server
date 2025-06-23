using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Sentyll.Domain.Common.Abstractions.Contracts.Queues;

namespace Sentyll.Domain.Common.Abstractions.Queues;

public sealed class UnboundedInMemoryQueue<TEventType> : IUnboundedInMemoryQueue<TEventType>
{
    
    private readonly Channel<TEventType> _channel;
    
    private ChannelWriter<TEventType> Writer => _channel.Writer;
    
    private ChannelReader<TEventType> Reader => _channel.Reader;

    public UnboundedInMemoryQueue(UnboundedChannelOptions queueOptions)
    {
        ArgumentNullException.ThrowIfNull(queueOptions);
        _channel = Channel.CreateUnbounded<TEventType>(queueOptions);
    }
    
    public async ValueTask PublishAsync(TEventType topic, CancellationToken cancellationToken = default) 
        => await Writer
            .WriteAsync(topic, cancellationToken)
            .ConfigureAwait(false);

    public ValueTask<TEventType> ReadAsync(CancellationToken cancellationToken = default)
        => Reader.ReadAsync(cancellationToken);
    
    public async IAsyncEnumerable<TEventType> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (!IsCleared())
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return await ReadAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public IAsyncEnumerable<TEventType> ContinuallyReadAllAsync(CancellationToken cancellationToken = default)
        => Reader.ReadAllAsync(cancellationToken);

    public bool IsCleared()
    {
        //Cannot read OR Count is 0 should return true.
        return !Reader.CanCount || Reader.Count == 0;
    }
}