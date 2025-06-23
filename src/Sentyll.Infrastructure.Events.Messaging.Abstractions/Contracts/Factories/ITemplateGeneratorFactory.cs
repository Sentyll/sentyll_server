using CSharpFunctionalExtensions;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Services.Templating;
using Sentyll.Infrastructure.Events.Messaging.Abstractions.Enums;

namespace Sentyll.Infrastructure.Events.Messaging.Abstractions.Contracts.Factories;

public interface ITemplateGeneratorFactory
{
    Result<ITemplateGenerator> Resolve(TemplateGeneratorType type);
}