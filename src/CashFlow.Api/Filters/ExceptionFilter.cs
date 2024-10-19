using CashFlow.Domain.Responses.Error;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters
{
    /// <summary>
    /// Filter
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// Exception treatment
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is CashFlowException)
            {
                HandleProjectException(context);
            } 
            else
            {
                ThrowUnknownError(context);
            }
        }

        private static void HandleProjectException(ExceptionContext context)
        {
            var cashflowException = context.Exception as CashFlowException;
            var errorResponse = new ErrorResponse(cashflowException!.GetErrors());

            context.HttpContext.Response.StatusCode = cashflowException.StatusCode;
            context.Result = new ObjectResult(errorResponse);
        }

        private static void ThrowUnknownError(ExceptionContext context)
        {
            var errorResponse = new ErrorResponse(ErrorMessageResource.UNKNOWN_ERROR);

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(errorResponse);
        }
    }
}
