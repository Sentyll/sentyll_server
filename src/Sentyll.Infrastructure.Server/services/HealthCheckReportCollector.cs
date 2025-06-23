using Sentyll.Infrastructure.Server.Core.Constants;
using Sentyll.Infrastructure.Server.Core.Contracts.Services;

namespace Sentyll.Infrastructure.Server.services;

internal sealed class HealthCheckReportCollector : IHealthCheckReportCollector, IDisposable
{
//     private readonly SentyllContext _db;
//     private readonly IHealthCheckFailureNotifier _healthCheckFailureNotifier;
//     private readonly ILogger<HealthCheckReportCollector> _logger;
//     private readonly IServerAddressesService _serverAddressService;
//     private readonly Settings _settings;
//     private static readonly Dictionary<int, Uri> _endpointAddresses = new();
//     private static readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web)
//     {
//         Converters =
//         {
//             // allowIntegerValues: true https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/issues/1422
//             new JsonStringEnumConverter(namingPolicy: null, allowIntegerValues: true)
//         }
//     };
//
//     public HealthCheckReportCollector(
//         SentyllContext db,
//         IHealthCheckFailureNotifier healthCheckFailureNotifier,
//         ILogger<HealthCheckReportCollector> logger,
//         IOptions<Settings> settings,
//         IServerAddressesService serverAddressService)
//     {
//         ArgumentNullException.ThrowIfNull(db);
//         ArgumentNullException.ThrowIfNull(healthCheckFailureNotifier);
//         ArgumentNullException.ThrowIfNull(logger);
//         ArgumentNullException.ThrowIfNull(serverAddressService);
//         ArgumentNullException.ThrowIfNull(settings);
//         
//         _db = db;
//         _healthCheckFailureNotifier = healthCheckFailureNotifier;
//         _logger = logger;
//         _serverAddressService = serverAddressService;
//         _settings = settings.Value;
//     }
//
//     public async Task Collect(CancellationToken cancellationToken)
//     {
//         using (_logger.BeginScope("HealthReportCollector is collecting health checks results."))
//         {
//             var healthChecks = await _db.Configurations.ToListAsync(cancellationToken).ConfigureAwait(false);
//
//             foreach (var item in healthChecks.OrderBy(h => h.Id))
//             {
//                 if (cancellationToken.IsCancellationRequested)
//                 {
//                     _logger.LogDebug("HealthReportCollector has been cancelled.");
//                     break;
//                 }
//
//                 foreach (var interceptor in _interceptors)
//                 {
//                     await interceptor.OnCollectExecuting(item).ConfigureAwait(false);
//                 }
//
//                 var healthReport = await GetHealthReportAsync(item).ConfigureAwait(false);
//
//                 if (healthReport.Status != HealthCheckStatus.Healthy)
//                 {
//                     if (!_settings.NotifyUnHealthyOneTimeUntilChange || await ShouldNotifyAsync(item.Name).ConfigureAwait(false))
//                     {
//                         await _healthCheckFailureNotifier.NotifyDown(item.Name, healthReport).ConfigureAwait(false);
//                     }
//                 }
//                 else
//                 {
//                     if (await HasLivenessRecoveredFromFailureAsync(item).ConfigureAwait(false))
//                     {
//                         await _healthCheckFailureNotifier.NotifyWakeUp(item.Name).ConfigureAwait(false);
//                     }
//                 }
//
//                 await SaveExecutionHistoryAsync(item, healthReport).ConfigureAwait(false);
//
//                 foreach (var interceptor in _interceptors)
//                 {
//                     await interceptor.OnCollectExecuted(healthReport).ConfigureAwait(false);
//                 }
//             }
//
//             _logger.LogDebug("HealthReportCollector has completed.");
//         }
//     }

//     private async Task<HealthStatusReport> GetHealthReportAsync(HealthCheckConfiguration configuration)
//     {
//         var (uri, name) = configuration;
//
//         try
//         {
//             var absoluteUri = GetEndpointUri(configuration);
//             HttpResponseMessage? response = null;
//
//             if (!string.IsNullOrEmpty(absoluteUri.UserInfo))
//             {
//                 var userInfoArr = absoluteUri.UserInfo.Split(':');
//                 if (userInfoArr.Length == 2 && !string.IsNullOrEmpty(userInfoArr[0]) && !string.IsNullOrEmpty(userInfoArr[1]))
//                 {
//                     //_httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(userInfoArr[0], userInfoArr[1]);
//
//                     // To support basic auth; we can add an auth header to _httpClient, in the DefaultRequestHeaders (as above commented line).
//                     // This would then be in place for the duration of the _httpClient lifetime, with the auth header present in every
//                     // request. This also means every call to GetHealthReportAsync should check if _httpClient's DefaultRequestHeaders
//                     // has already had auth added.
//                     // Otherwise, if you don't want to effect _httpClient's DefaultRequestHeaders, then you have to explicitly create
//                     // a request message (for each request) and add/set the auth header in each request message. Doing the latter
//                     // means you can't use _httpClient.GetAsync and have to use _httpClient.SendAsync
//
//                     using var requestMessage = new HttpRequestMessage(HttpMethod.Get, absoluteUri);
//                     requestMessage.Headers.Authorization = new BasicAuthenticationHeaderValue(userInfoArr[0], userInfoArr[1]);
//                     response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
//                 }
//             }
//
//             response ??= await _httpClient.GetAsync(absoluteUri, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
//
//             using (response)
//             {
//                 if (!response.IsSuccessStatusCode && response.Content.Headers.ContentType?.MediaType != "application/json")
//                     return HealthStatusReport.CreateFrom(new InvalidOperationException($"HTTP response is not in valid state ({response.StatusCode}) when trying to get report from {uri} configured with name {name}."));
//
//                 return await response.Content.ReadFromJsonAsync<HealthStatusReport>(_options).ConfigureAwait(false)
//                     ?? throw new InvalidOperationException($"{nameof(HttpContentJsonExtensions.ReadFromJsonAsync)} returned null");
//             }
//         }
//         catch (Exception exception)
//         {
//             _logger.LogError(exception, $"GetHealthReport threw an exception when trying to get report from {uri} configured with name {name}.");
//
//             return HealthStatusReport.CreateFrom(exception);
//         }
//     }
//
//     private Uri GetEndpointUri(HealthCheckConfiguration configuration)
//     {
//         if (_endpointAddresses.ContainsKey(configuration.Id))
//         {
//             return _endpointAddresses[configuration.Id];
//         }
//
//         Uri.TryCreate(configuration.Uri, UriKind.Absolute, out var absoluteUri);
//
//         if (absoluteUri == null || !absoluteUri.IsValidHealthCheckEndpoint())
//         {
//             Uri.TryCreate(_serverAddressService.AbsoluteUriFromRelative(configuration.Uri), UriKind.Absolute, out absoluteUri);
//         }
//
//         if (absoluteUri == null)
//             throw new InvalidOperationException("Could not get endpoint uri from configuration");
//
//         _endpointAddresses[configuration.Id] = absoluteUri;
//
//         return absoluteUri;
//     }
//
//     private async Task<bool> HasLivenessRecoveredFromFailureAsync(HealthCheckConfiguration configuration)
//     {
//         var previous = await GetHealthCheckExecutionAsync(configuration).ConfigureAwait(false);
//
//         return previous != null && previous.Status != HealthCheckStatus.Healthy;
//     }
//
//     private async Task<HealthCheckExecution?> GetHealthCheckExecutionAsync(HealthCheckConfiguration configuration)
//     {
//         return await _db.Executions
//             .Include(le => le.History)
//             .Include(le => le.Entries)
//             .AsSplitQuery()
//             .Where(le => le.Name == configuration.Name)
//             .SingleOrDefaultAsync()
//             .ConfigureAwait(false);
//     }
//
//     private async Task<bool> ShouldNotifyAsync(string healthCheckName)
//     {
// #pragma warning disable RCS1155 // Use StringComparison when comparing strings, see https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/pull/1995
//         var lastNotifications = await _db.Failures
//            .Where(lf => string.Equals(lf.HealthCheckName, healthCheckName))
//            .OrderByDescending(lf => lf.LastNotified)
//            .Take(2).ToListAsync().ConfigureAwait(false);
// #pragma warning restore RCS1155 // Use StringComparison when comparing strings.
//
//         if (lastNotifications?.Count == 2)
//         {
//             var first = lastNotifications[0];
//             var second = lastNotifications[1];
//             if (first.IsUpAndRunning == second.IsUpAndRunning)
//             {
//                 return false;
//             }
//         }
//
//         return true;
//     }
//
//     private async Task SaveExecutionHistoryAsync(HealthCheckConfiguration configuration, HealthStatusReport healthReport)
//     {
//         _logger.LogDebug("HealthReportCollector - health report execution history saved.");
//
//         var execution = await GetHealthCheckExecutionAsync(configuration).ConfigureAwait(false);
//
//         var lastExecutionTime = DateTime.UtcNow;
//
//         if (execution != null)
//         {
//             if (execution.Uri != configuration.Uri)
//             {
//                 UpdateUris(execution, configuration);
//             }
//
//             if (execution.Status == healthReport.Status)
//             {
//                 _logger.LogDebug("HealthReport history already exists and is in the same state, updating the values.");
//
//                 execution.LastExecuted = lastExecutionTime;
//             }
//             else
//             {
//                 SaveExecutionHistoryEntries(healthReport, execution, lastExecutionTime);
//             }
//
//             // update existing entries with values from new health report
//
//             foreach (var item in healthReport.ToExecutionEntries())
//             {
//                 var existing = execution.Entries
//                     .SingleOrDefault(e => e.Name == item.Name);
//
//                 if (existing != null)
//                 {
//                     existing.Status = item.Status;
//                     existing.Description = item.Description;
//                     existing.Duration = item.Duration;
//                     existing.Tags = item.Tags;
//                 }
//                 else
//                 {
//                     execution.Entries.Add(item);
//                 }
//             }
//
//             // remove old entries if existing execution not present in new health report
//
//             foreach (var item in execution.Entries)
//             {
//                 if (!healthReport.Entries.ContainsKey(item.Name))
//                     _db.HealthCheckExecutionEntries.Remove(item);
//             }
//         }
//         else
//         {
//             _logger.LogDebug("Creating a new HealthReport history.");
//
//             execution = new HealthCheckExecution
//             {
//                 LastExecuted = lastExecutionTime,
//                 OnStateFrom = lastExecutionTime,
//                 Entries = healthReport.ToExecutionEntries(),
//                 Status = healthReport.Status,
//                 Name = configuration.Name,
//                 Uri = configuration.Uri,
//                 DiscoveryService = configuration.DiscoveryService
//             };
//
//             await _db.Executions
//                 .AddCronJobAsync(execution)
//                 .ConfigureAwait(false);
//         }
//
//         await _db.SaveChangesAsync().ConfigureAwait(false);
//     }
//
//     private static void UpdateUris(HealthCheckExecution execution, HealthCheckConfiguration configuration)
//     {
//         execution.Uri = configuration.Uri;
//         _endpointAddresses.Remove(configuration.Id);
//     }
//
//     private void SaveExecutionHistoryEntries(HealthStatusReport healthReport, HealthCheckExecution execution, DateTime lastExecutionTime)
//     {
//         _logger.LogDebug("HealthCheckReportCollector already exists but on different state, updating the values.");
//
//         foreach (var item in execution.Entries)
//         {
//             // If the health service is down, no entry in dictionary
//             if (healthReport.Entries.TryGetValue(item.Name, out var reportEntry))
//             {
//                 if (item.Status != reportEntry.Status)
//                 {
//                     execution.History.Add(new HealthCheckExecutionHistory
//                     {
//                         On = lastExecutionTime,
//                         Status = reportEntry.Status,
//                         Name = item.Name,
//                         Description = reportEntry.Description
//                     });
//                 }
//             }
//         }
//
//         execution.OnStateFrom = lastExecutionTime;
//         execution.LastExecuted = lastExecutionTime;
//         execution.Status = healthReport.Status;
//     }












     private readonly HttpClient _httpClient;

     private bool _disposed;

     public HealthCheckReportCollector(
         IHttpClientFactory httpClientFactory
         )
     {
         _httpClient = httpClientFactory.CreateClient(DependencyConstants.ReportCollectorHttpClient);
     }
     
    public Task Collect(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _httpClient.Dispose();
        _disposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

}
