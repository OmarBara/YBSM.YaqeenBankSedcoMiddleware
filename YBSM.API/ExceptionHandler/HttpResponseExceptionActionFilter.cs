using System;
using System.Net;
using Core.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.API.ExceptionHandler
{
    public class HttpResponseExceptionActionFilter : IActionFilter, IOrderedFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is APIException exception)
            {
                context.Result = new ObjectResult(exception.Value)
                {
                    StatusCode = (int) exception.Status
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception is Exception)
            {
                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError
                };
                context.ExceptionHandled = true;
            }
        }

        public int Order { get; set; } = int.MaxValue - 10;
    }
}