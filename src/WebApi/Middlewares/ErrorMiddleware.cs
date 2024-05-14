using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middlewares;

[ExcludeFromCodeCoverage]
public class ErrorMiddleware
{
	private static readonly JsonSerializerOptions _serializerOptions = new();
	private readonly ILogger<ErrorMiddleware> _logger;
	private readonly RequestDelegate _next;

	public ErrorMiddleware(
		RequestDelegate next,
		ILogger<ErrorMiddleware> logger
	)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (DomainException exception)
		{
			_logger.LogWarning(exception, exception.Message);

			var statusCodeResponse = HttpStatusCode.BadRequest;
			var problemDetails = BuildProblemDetails(context.Request.Path, exception.Message, statusCodeResponse);
			await HandleResponseAsync(context, (int)statusCodeResponse, problemDetails);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, exception.Message);

			var statusCodeResponse = HttpStatusCode.BadRequest;
			var problemDetails = BuildProblemDetails(context.Request.Path, exception.Message, statusCodeResponse);
			await HandleResponseAsync(context, (int)statusCodeResponse, problemDetails);
		}
	}

	private static async Task HandleResponseAsync<T>(
		HttpContext context,
		int statusCodeResponse,
		T response
	)
	{
		context.Response.StatusCode = statusCodeResponse;
		await context.Response.WriteAsJsonAsync(response, _serializerOptions);
	}

	private static ProblemDetails BuildProblemDetails(
		string instace,
		string detail,
		HttpStatusCode statusCode
	)
	{
		return new ProblemDetails
		{
			Title = statusCode.ToString(), Instance = instace, Status = (int)statusCode, Type = "about:blank"
		};
	}
}