using System.Net;

using AlzaApi.Common.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AlzaApi.App.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, message) = exception switch
        {
            NotFoundException => ((int) HttpStatusCode.NotFound, exception.Message),
            _ => ((int) HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new ProblemDetails
        {
            Status = statusCode,
            Title = message
        };

        await context.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
