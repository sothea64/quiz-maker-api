using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using quiz_maker_api.Logics;
using quiz_maker_models.OutfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace quiz_maker_api.Helpers
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.BadRequest; // 400 if unexpected 
            var obj = new HandleException()
            {
                Error = exception.InnerException?.Message ?? exception.Message,
                Type = exception.GetType().FullName,
                Stack = exception.StackTrace,
            };
            if (exception is LogicException)
            {
                obj.Code = (exception as LogicException).Code;
            }
            else if (exception is UnAuthorizeException)
            {
                code = HttpStatusCode.Unauthorized; //401 un-authorize
            }
            var result = JsonConvert.SerializeObject(obj);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
