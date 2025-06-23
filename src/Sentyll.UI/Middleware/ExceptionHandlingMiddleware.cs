using System.Net;
using Sentyll.UI.Core.Models.Failure;

namespace Sentyll.UI.Middleware;

internal sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // proceed down the pipeline
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new InternalServerFailureResult(
                "UNEO", 
                "An unexpected error occurred.",
                (int)HttpStatusCode.BadRequest,
                _env.IsDevelopment() ? ex.ToString() : null,
                context.TraceIdentifier);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
