using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GloboTicket.TicketManagement.Application.Exceptions;
using Newtonsoft.Json;

namespace GloboTicket.TicketManagement.Api.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await ConvertException(httpContext, ex);
            }
        }
        private Task ConvertException(HttpContext httpContext, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            httpContext.Response.ContentType = "application/json";

            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(validationException.ValidationErrors);
                    break;
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = badRequestException.Message;
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    result = notFoundException.Message;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    result = exception.Message;
                    while (exception.InnerException != null)
                    {
                        result += exception.InnerException.Message;
                        exception = exception.InnerException;
                    }
                    break;
            }

            httpContext.Response.StatusCode = (int)statusCode;

            if (string.IsNullOrEmpty(result))
            {
                result = JsonConvert.SerializeObject(new {error = exception.Message});
            }

            return httpContext.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
