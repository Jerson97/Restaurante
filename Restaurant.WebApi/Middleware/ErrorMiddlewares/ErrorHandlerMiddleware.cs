using System.Net;
using System.Text.Json;
using Restaurant.Domain.Models;
using Restaurant.Domain.Result;

namespace Restaurant.WebApi.Middleware.ErrorMiddlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ErrorHandlerAsync(context, ex);
            }
        }

        private async Task ErrorHandlerAsync(HttpContext context, Exception ex)
        {
            string message = null;

            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case ErrorHandler eh:

                    context.Response.StatusCode = (int)eh.Code;
                    message = eh.Message;
                    break;

                case Exception e:

                    message = string.IsNullOrWhiteSpace(e.Message) ? "Error desconocido" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    break;
            }
            int customResponse = 0;
            if (message != null)
            {

                switch (context.Response.StatusCode)
                {
                    case 404:
                        customResponse = 0;
                        break;

                    case 200:
                        customResponse = 1;
                        break;

                    case 422:
                        customResponse = 2;
                        break;
                }

                await context.Response.WriteAsync(JsonSerializer.Serialize(MessageResult<object>.Of(message, ex.Data, customResponse)));
            }
        }
    }
}
