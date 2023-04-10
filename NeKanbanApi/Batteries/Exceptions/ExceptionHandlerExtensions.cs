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
                await WriteToBody(context.Response, e.Message);
            }
            catch (HttpStatusCodeException e)
            {
                context.Response.StatusCode = (int) e.Status;
                await WriteToBody(context.Response, e.Message);
            }
        });
    }

    private static async Task WriteToBody(HttpResponse response, string str)
    {
        response.ContentType = "text/plain";
        await response.WriteAsync(str);
    }
}