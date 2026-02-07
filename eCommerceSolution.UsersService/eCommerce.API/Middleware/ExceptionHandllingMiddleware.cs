namespace eCommerce.API.Middleware;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class ExceptionHandllingMiddleware(RequestDelegate next, ILogger<ExceptionHandllingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandllingMiddleware> _logger = logger;

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            // Log the excpetion type and message
            _logger.LogError($"{ex.GetType}: {ex.Message}");

            if (ex.InnerException is not null)
            {
                // Log the inner exception type and message
                _logger.LogError($"{ex.InnerException.GetType}: {ex.InnerException.Message}");
            }

            // Set the response status code to 500 Internal Server Error
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { ex.Message, Type = ex.GetType().ToString() });
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class ExceptionHandllingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandllingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandllingMiddleware>();
    }
}
