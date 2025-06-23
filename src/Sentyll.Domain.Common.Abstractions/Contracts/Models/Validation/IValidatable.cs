namespace Sentyll.Domain.Common.Abstractions.Contracts.Models.Validation;

public interface IValidatable
{
    /// <summary>
    /// Validates the internal state of the implementation type.
    /// </summary>
    /// <returns></returns>
    Result Validate();
}