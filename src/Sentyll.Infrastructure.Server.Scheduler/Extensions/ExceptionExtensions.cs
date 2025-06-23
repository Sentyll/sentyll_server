namespace Sentyll.Infrastructure.Server.Scheduler.Extensions;

internal static class ExceptionExtensions
{
    public static Exception GetRootException(this Exception ex)
    {
        while (ex.InnerException != null)
        {
            ex = ex.InnerException;
        }
        
        return ex;
    }
}