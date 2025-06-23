using CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.models.Request;

namespace Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Services.Templating;

public interface ITemplateGenerator
{

    public Task<Result<string>> GenerateUpAsync(
        GenerateTemplateRequest eventRequest, 
        CancellationToken cancellationToken = default
    );
    
    public Task<Result<string>> GenerateDownAsync(
        GenerateTemplateRequest eventRequest, 
        CancellationToken cancellationToken = default
    );

}