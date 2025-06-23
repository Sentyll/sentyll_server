using CSharpFunctionalExtensions;

namespace Sentyll.UI.Core.Extensions;

public static class IActionResultExtensions
{

    public static IActionResult OkOrFailure<TResult>(this Result<TResult> result)
    {
        return result.IsSuccess ? new OkObjectResult(result.Value) : new BadRequestObjectResult(result.Error);
    }
    
    public static IActionResult OkOrFailureAsync(this Result result)
    {
        return result.IsSuccess ? new OkResult() : new BadRequestObjectResult(result.Error);
    }
    
    public static async Task<IActionResult> OkOrFailureAsync<TResult>(this Task<Result<TResult>> awaitableResult)
    {
        var result = await awaitableResult;
        return result.IsSuccess ? new OkObjectResult(result.Value) : new BadRequestObjectResult(result.Error);
    }
    
    public static async Task<IActionResult> OkOrFailureAsync(this Task<Result> awaitableResult)
    {
        var result = await awaitableResult;
        return result.IsSuccess ? new OkResult() : new BadRequestObjectResult(result.Error);
    }
}