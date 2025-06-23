namespace Sentyll.Infrastructure.Server.Abstractions.Contracts.Services.Address;

public interface IServerAddressesService
{
    string ServerAddress();
    string AbsoluteUriFromRelative(string relativeUrl);
}