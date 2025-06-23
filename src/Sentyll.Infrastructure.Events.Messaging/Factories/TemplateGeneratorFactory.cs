using CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Factories;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Services.Templating;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Enums;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Failures;

namespace Sentyll.Infrastructure.Events.Messaging.Factories;

internal sealed class TemplateGeneratorFactory : Dictionary<TemplateGeneratorType, ITemplateGenerator>, ITemplateGeneratorFactory
{
    
    public Result<ITemplateGenerator> Resolve(TemplateGeneratorType type)
        => Result
            .FailureIf(!TryGetValue(type, out ITemplateGenerator? generator), TemplateGeneratorFactoryFailures.NotFound.ToString())
            .Map(() => generator!);

}