using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace BackendCRUD.Minimal.Api.ErrorHandler
{
    public sealed class ExceptionHandlingMiddleware
    {
        //private readonly RequestDelegate _next;
        //ILogger<ExceptionHandlingMiddleware> _logger;

        //public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        //{
        //    _next = next;
        //    _logger = logger;
        //}

        //public async Task InvokeAsync(HttpContext context)
        //{
        //    try
        //    {
        //        await _next(context);
        //    }
        //    catch (Exceptions.ValidationException exception)
        //    {
        //        var problemDetails = new ProblemDetails
        //        {
        //            Status = StatusCodes.Status400BadRequest,
        //            Type = "ValidationFailure",
        //            Title = "Validation error",
        //            Detail = "One or more validation errors has occurred"
        //        };

        //        if (exception.Errors is not null)
        //        {
        //            problemDetails.Extensions["errors"] = exception.Errors;
        //        }

        //        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        //        await context.Response.WriteAsJsonAsync(problemDetails);
        //    }
        //}
    }
}
