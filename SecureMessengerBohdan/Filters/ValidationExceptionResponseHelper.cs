using Microsoft.AspNetCore.Mvc;
using SecureMessengerBohdan.Application.Wrappers;

namespace SecureMessengerBohdan.Filters
{
    public static class ValidationExceptionResponseHelper
    {
        public static Func<ActionContext, IActionResult> Factory => context =>
        {
            return new BadRequestObjectResult(
                new ResultWrapper(
                    context.ModelState
                    .SelectMany(keyValue => 
                    keyValue.Value.Errors
                    .Select(err => err.ErrorMessage)    
                    )
                    .ToArray()));
        };
    }
}
