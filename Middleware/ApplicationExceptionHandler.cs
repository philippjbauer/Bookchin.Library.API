using System;
using System.Diagnostics;
using System.Text.Json;
using Bookchin.Library.API.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Bookchin.Library.API.Middleware
{
    public class ApplicationExceptionHandler
    {
        public static void Invoke(IApplicationBuilder errorApp)
        {
            errorApp.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature.Error;

                // https://tools.ietf.org/html/rfc7807#section-3.1
                var problemDetails = new ProblemDetails
                {
                    Type = $"{exception.GetType().Name}",
                    Title = "An unexpected error occurred!",
                    Detail = exception.Message,
                    Instance = errorFeature switch
                    {
                        ExceptionHandlerFeature e => e.Path,
                        _ => "unknown"
                    },
                    Status = StatusCodes.Status500InternalServerError,
                    Extensions =
                    {
                        ["trace"] = Activity.Current?.Id ?? context?.TraceIdentifier
                    }
                };

                switch (exception)
                {
                    case ModelValidationException modelValidationException:
                        problemDetails.Status = StatusCodes.Status500InternalServerError;
                        problemDetails.Title = "Missing required model property.";
                        problemDetails.Detail = "An internal model instance was encountered that does not contain values for all required properties.";
                        problemDetails.Extensions["errors"] = modelValidationException.Errors;
                        break;
                    case ModelRecordNotFoundException modelRecordNotFoundException:
                        problemDetails.Status = StatusCodes.Status404NotFound;
                        problemDetails.Title = "Model record not found.";
                        break;
                }

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = problemDetails.Status.Value;
                context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                {
                    NoCache = true,
                };

                await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails);
            });
        }
    }
}