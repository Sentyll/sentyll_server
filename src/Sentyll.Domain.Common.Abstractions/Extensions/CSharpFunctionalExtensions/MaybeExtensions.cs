namespace Sentyll.Domain.Common.Abstractions.Extensions.CSharpFunctionalExtensions;

public static class MaybeExtensions
{
    public static async Task<Maybe<DateTime>> AsMaybe(this Task<DateTime> defaultDateTimeTask) 
        => Maybe.From(await defaultDateTimeTask.ConfigureAwait(false));
}