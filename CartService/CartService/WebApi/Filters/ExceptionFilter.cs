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
            switch (context.Exception)
            {
                case NotFoundException notFoundException:
                    context.Result = new NotFoundObjectResult(new { error = notFoundException.Message });
                    context.ExceptionHandled = true;
                    break;

                default:
                    if (FindDomainException(context.Exception) is DomainException domainException)
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

                    break;
            }
        }

        private static DomainException? FindDomainException(Exception? ex)
        {
            switch (ex)
            {
                case null:
                    return null;
                case DomainException domainEx:
                    return domainEx;
                default:
                    return FindDomainException(ex.InnerException);
            }
        }
    }
}
