using GuanajuatoAdminUsuarios.Models.Commons;
using GuanajuatoAdminUsuarios.Utils.Interfaces;
using GuanajuatoAdminUsuarios.Utils.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace GuanajuatoAdminUsuarios.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Guid ReqId = Guid.NewGuid();
                        await new LogService().ErrorAsync(contextFeature.Error, ReqId);
#if DEBUG
                        await context.Response.WriteAsync(new ResponseErrorTechnical()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Trace = contextFeature.Error.StackTrace
                        }.ToString());
#else
                        await context.Response.WriteAsync(new ResponseError()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. RequestId: " + ReqId.ToString();
                        }.ToString());
#endif
                    }
                });
            });
        }
    }
}
