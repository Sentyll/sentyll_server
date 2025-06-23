namespace Sentyll.Infrastructure.Server.Abstractions.Contracts.Services;

public interface ISystemClock
{
    DateTime UtcNow { get; }
}