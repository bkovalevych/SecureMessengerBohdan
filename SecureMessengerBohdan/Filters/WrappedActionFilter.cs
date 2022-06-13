using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SecureMessengerBohdan.Application.Exceptions;
using SecureMessengerBohdan.Application.Wrappers;

namespace SecureMessengerBohdan.Filters
{
    public class WrappedActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null && context.Result is ObjectResult result)
            {
                var wrappedResult = new ResultWrapper()
                {
                    Result = result.Value
                };
                context.Result = new OkObjectResult(wrappedResult);
            }
            if (context.Exception is DomainException domainException)
            {
                context.Result = new BadRequestObjectResult(new ResultWrapper(domainException.Messages.ToArray()));
                context.ExceptionHandled = true;
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ResultWrapper(context.ModelState.Select(it => $"{it.Key} {it.Value}").ToArray()));
            }
        }


    }
}
