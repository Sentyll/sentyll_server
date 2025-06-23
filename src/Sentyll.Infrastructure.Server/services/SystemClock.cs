namespace Sentyll.Infrastructure.Server.services;

internal sealed class SystemClock : ISystemClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}