using Catalog.Core.Exceptions;

namespace Catalog.WebAPI.Middlewares
{
    /// <summary>
    /// Middleware for exception handling.
    /// </summary>
    /// <param name="next">Next.</param>
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        /// <summary>
        /// Invoke method.
        /// </summary>
        /// <param name="context">Http context.</param>
        /// <returns>Task.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                var domainEx = ex as DomainException ?? ex.InnerException as DomainException;

                if (domainEx != null)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = domainEx.Message });
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
                }
            }
        }
    }
}
