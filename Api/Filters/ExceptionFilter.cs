using Application.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class ExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue;

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is DetailedException exception)
            {
                context.Result = new ObjectResult(exception.ErrorResponse)
                {
                    StatusCode = (int) exception.StatusCode
                };
                
                context.ExceptionHandled = true;
            }
        }
    }
}