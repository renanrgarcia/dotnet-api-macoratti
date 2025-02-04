using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanArch.API.Filters;

public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is FluentValidation.ValidationException validationException)
        {
            // Treat validation errors as 400 Bad Request
            var errors = validationException.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                .ToList();

            var result = new ObjectResult(new { Errors = errors })
            {
                StatusCode = 400
            };

            context.Result = result;
            context.ExceptionHandled = true;
        }

        else if (context.Exception is ArgumentNullException || context.Exception is KeyNotFoundException)
        {
            // Treat errors as 404 Not Found
            context.Result = new NotFoundObjectResult(new { Error = "Resource not found" });
            context.ExceptionHandled = true;
        }

        else if (context.Exception is HttpRequestException || context.Exception is InvalidOperationException)
        {
            // Treat errors as 500 Internal Server Error
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            context.ExceptionHandled = true;
        }
    }
}
