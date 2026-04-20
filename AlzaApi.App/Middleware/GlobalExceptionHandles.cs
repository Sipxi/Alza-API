using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using AlzaApi.Common.Exceptions; 

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
            NotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new 
        { 
            status = statusCode,
            message = message 
        };

        await context.Response.WriteAsJsonAsync(response, cancellationToken);

        // Возвращаем true, чтобы конвейер знал, что ошибка обработана
        return true;
    }
}
