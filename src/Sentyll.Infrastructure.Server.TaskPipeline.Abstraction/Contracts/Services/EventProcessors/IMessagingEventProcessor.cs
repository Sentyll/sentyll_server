using Sentyll.Domain.Common.Abstractions.Enums;

namespace Sentyll.Infrastructure.Server.TaskPipeline.Abstraction.Contracts.Services.EventProcessors;

/// <summary>
/// Reserved Processor Type that handled Notification Executions
/// </summary>
public interface IMessagingEventProcessor : IEventProcessor
{
    public MessagingEventType Type { get; }
}