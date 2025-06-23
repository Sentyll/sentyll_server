using Sentyll.Domain.Common.Abstractions.Exceptions;

namespace Sentyll.Domain.Common.Abstractions.Failures;

public record Failure(string Code, string Number, string Message) : IError<string>
{
    
    public string Error => ToString();

    public static implicit operator Exception(Failure failure) => new (failure.ToString());
    
    public static implicit operator string(Failure failure) => new (failure.ToString());
    
    public override string ToString() 
        => string.Format("[{0}:{1}]:{2}", Code, Number, Message);
    
    public static Failure FromMessage(string error)
    {
        var segments = error
            .Replace("[", string.Empty)
            .Replace("]", string.Empty)
            .Split(":", 3);
        
        if(segments.Length != 3)
        {
            throw new RottenBanana($"Cannot construct a Failure from error message [{error}]");
        }

        return new Failure(segments[0], segments[1], segments[2]);
    }
    
}