﻿using System.Net;
using Microsoft.AspNetCore.Http;
using ProCrew.Application.Common.Exceptions;
using ProCrew.Application.Common.Services;
using Serilog;
using Serilog.Context;
using ValidationException = FluentValidation.ValidationException;

namespace ProCrew.Infrastructure.Middlewares;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ISerializerService _jsonSerializer;

    public ExceptionMiddleware(
        ISerializerService jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var errorId = Guid.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);
            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim(),
                ErrorId = errorId,
                SupportMessage = string.Format("Provide the ErrorId {0} to the support team for further analysis.",
                    errorId)
            };

            if (exception is not CustomException && exception.InnerException != null)
                while (exception.InnerException != null)
                    exception = exception.InnerException;

            if (exception is ValidationException fluentException)
            {
                errorResult.Exception = "One or More Validations failed.";
                foreach (var error in fluentException.Errors) errorResult.Messages.Add(error.ErrorMessage);
            }

            switch (exception)
            {
                case CustomException e:
                    errorResult.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null) errorResult.Messages = e.ErrorMessages;

                    break;

                case KeyNotFoundException:
                    errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ValidationException:
                    errorResult.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error(
                $"{errorResult.Exception} Request failed with Status Code {errorResult.StatusCode} and Error Id {errorId}.");
            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;
                await response.WriteAsync(_jsonSerializer.Serialize(errorResult));
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }
        }
    }
}