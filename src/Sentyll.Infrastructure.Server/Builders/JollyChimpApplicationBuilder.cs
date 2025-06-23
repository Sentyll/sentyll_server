using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Sentyll.Infrastructure.Server.Builders;

public class SentyllApplicationBuilder
{
    
    private readonly WebApplication _webApplication;

    public SentyllApplicationBuilder(WebApplication webApplication)
    {
        _webApplication = webApplication;
    }
    
    public SentyllApplicationBuilder MapEndpoints(Action<IEndpointRouteBuilder> configureEndpointsAction)
    {
        configureEndpointsAction(
            _webApplication
        );
        
        return this;
    }
    
    public SentyllApplicationBuilder UseServices(Action<IApplicationBuilder> configureServicesAction)
    {
        configureServicesAction(
            _webApplication
        );
        
        return this;
    }
    
}