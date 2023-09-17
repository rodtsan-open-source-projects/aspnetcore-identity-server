using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AspNetCore.Identity.Infrastructure.Middlewares
{
	public sealed class ExceptionHandlingMiddleware : IMiddleware
	{
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;
		public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				await HandleExceptionAsync(context, e);
			}
		}
		private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
		{
			var statusCode = GetStatusCode(exception);
			var response = new
			{
				title = GetTitle(exception),
				status = statusCode,
				detail = exception.Message,
				errors = GetErrors(exception)
			};
			httpContext.Response.ContentType = "application/json";
			httpContext.Response.StatusCode = statusCode;
			await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
		}
		private static int GetStatusCode(Exception exception) => exception.HResult;
		private static string GetTitle(Exception exception) =>
			exception switch
			{
				ApplicationException applicationException => applicationException.Message,
				_ => "Server Error"
			};
		private static IEnumerable<string> GetErrors(Exception exception)
		{
			IEnumerable<string> errors = new List<string>();
			if (exception is ValidationException validationException)
			{
				var errorMessages = validationException.Errors.Select(q => q.ErrorMessage).ToArray();
				errors = validationException.Errors.Select(q => q.ErrorMessage);
			}
			return errors;
		}
	}
}
