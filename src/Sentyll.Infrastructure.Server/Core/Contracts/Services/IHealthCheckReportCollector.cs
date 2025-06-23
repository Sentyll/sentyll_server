namespace Sentyll.Infrastructure.Server.Core.Contracts.Services;

public interface IHealthCheckReportCollector
{
    Task Collect(CancellationToken cancellationToken);
}
