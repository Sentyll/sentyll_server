using Sentyll.Infrastructure.Server.Abstractions.Contracts.Services.Address;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Sentyll.Infrastructure.Server.services.Address;

internal sealed class ServerAddressesService : IServerAddressesService
{
    
    private readonly IServer _server;

    public ServerAddressesService(IServer server)
    {
        _server = server;
    }

    private ICollection<string>? Addresses => AddressesFeature?.Addresses;

    private IServerAddressesFeature? AddressesFeature 
        => _server.Features.Get<IServerAddressesFeature>();

    public string ServerAddress()
    {
        var targetAddress = Addresses!.First();

        if (targetAddress.EndsWith("/"))
        {
            targetAddress = targetAddress[0..^1];
        }
        return targetAddress;
    }
    
    public string AbsoluteUriFromRelative(string relativeUrl)
    {
        var targetAddress = ServerAddress();

        if (!relativeUrl.StartsWith("/"))
        {
            relativeUrl = $"/{relativeUrl}";
        }

        return $"{targetAddress}{relativeUrl}";
    }
}
