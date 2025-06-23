using Sentyll.Domain.Common.Abstractions.Failures;

namespace Sentyll.Domain.Common.Abstractions.Exceptions;

/// <summary>
/// Throwing Exceptions are slightly discouraged in Sentyll.
/// The rotten banana exception should be used lightly for throwing errors where <see cref="Result"/> Validation is not possible
/// </summary>
public class RottenBanana : Exception
{
    public RottenBanana(string message) : base(message)
    {
        
    }
    
    public RottenBanana(Failure failure) : base(failure.ToString())
    {
        
    }
}