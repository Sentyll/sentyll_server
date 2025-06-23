using Sentyll.Domain.Common.Abstractions.Failures;

namespace Sentyll.Infrastructure.Events.Messaging.Abstractions.Failures;

public static class TemplateGeneratorFactoryFailures
{
    private const string Code = "TPGF";

    public static readonly Failure NotFound = new(Code, "0001", "Requested Generator was not found or registered properly.");
}