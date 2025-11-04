using CartApp.BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CartApp.WebApi.Filters
{
    /// <summary>
    /// Exception filter.
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// On Exception.
        /// </summary>
        /// <param name="context">Context.</param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException notFoundException)
            {
                context.Result = new NotFoundObjectResult(new { error = notFoundException.Message });
                context.ExceptionHandled = true;
            }
            else if (FindDomainException(context.Exception) is DomainException domainException)
            {
                context.Result = new BadRequestObjectResult(new { error = domainException.Message });
                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new ObjectResult(new { error = "Internal server error" })
                {
                    StatusCode = 500,
                };
                context.ExceptionHandled = true;
            }
        }

        private static DomainException? FindDomainException(Exception? ex)
        {
            while (ex != null)
            {
                if (ex is DomainException domainEx)
                {
                    return domainEx;
                }

                ex = ex.InnerException;
            }

            return null;
        }
    }
}
