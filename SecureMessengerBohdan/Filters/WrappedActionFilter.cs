using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
            if (context.Exception is ArgumentException argumentException)
            {
                context.Result = new BadRequestObjectResult(new ResultWrapper(argumentException.Message));
                context.ExceptionHandled = true;
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }


    }
}
