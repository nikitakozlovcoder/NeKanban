using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Batteries.Exceptions;

public static class ExceptionHandlerExtensions
{
    public static void UseAppExceptionHandler(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (EntityDoesNotExists e)
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                await context.Response.WriteAsync(e.Message);
            }
            catch (HttpStatusCodeException e)
            {
                context.Response.StatusCode = (int) e.Status;
                await context.Response.WriteAsync(e.Message);
            }
        });
    }
}