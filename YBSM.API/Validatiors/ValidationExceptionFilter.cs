using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            var errors = validationException.Errors.Select(error => new
            {
                Code = (error.CustomState as dynamic)?.Code ?? "E0",
                Message = (error.CustomState as dynamic)?.Message ?? error.ErrorMessage
            });

            context.Result = new JsonResult(new { Result = errors })
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            context.ExceptionHandled = true;
        }
    }
}
