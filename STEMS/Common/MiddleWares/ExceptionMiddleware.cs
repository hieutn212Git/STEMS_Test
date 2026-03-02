using Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace Common.MiddleWares
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var requestGuid = Guid.NewGuid().ToString();
            if (statusCode == 500)
            {
                Log.Logger.Error(exception, "[ERR_ID = {@requestGuid}] Something went wrong while handling request", requestGuid);
            }

            var response = new ApiResponse
            {
                StatusCode = statusCode,
                Message = statusCode == 500 ? $"Please contact your system administrator: {requestGuid}" : exception.Message,
                Errors = GetErrors(exception),
                Success = false
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private static int GetStatusCode(Exception exception)
        {
            if (exception is ValidationException || exception is InvalidValidationException)
            {
                return StatusCodes.Status400BadRequest;
            }
            else if (exception is UnauthorizedAccessException)
            {
                return StatusCodes.Status401Unauthorized;
            }
            else
            {
                return StatusCodes.Status500InternalServerError;
            }
        }

        private static IEnumerable<ValidationErrorResponse> GetErrors(Exception exception)
        {
            IEnumerable<ValidationErrorResponse> errors = default!;
            if (exception is ValidationException validationException)
            {
                errors = validationException.Errors
                    .Select(gs => new ValidationErrorResponse
                    {
                        ErrorCode = gs.ErrorCode,
                        PropertyName = gs.PropertyName,
                        ErrorMessage = gs.ErrorMessage
                    });
            }
            else if (exception is InvalidValidationException invalidationException)
            {
                errors = invalidationException.Errors
                    .Select(gs => new ValidationErrorResponse
                    {
                        ErrorCode = gs.ErrorCode,
                        PropertyName = gs.PropertyName,
                        ErrorMessage = gs.ErrorMessage?.ToString() ?? string.Empty
                    });
            }
            return errors;
        }
    }
}
