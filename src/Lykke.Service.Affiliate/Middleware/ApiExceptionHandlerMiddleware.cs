using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.ApiLibrary.Middleware;
using Lykke.Service.Affiliate.Models;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Lykke.Service.Affiliate.Middleware
{
    public class ApiExceptionHandlerMiddleware
    {
        private readonly ILog _log;
        private readonly string _componentName;
        private readonly RequestDelegate _next;

        public ApiExceptionHandlerMiddleware(RequestDelegate next, ILog log, string componentName)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _componentName = componentName ?? throw new ArgumentNullException(nameof(componentName));
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ApiException ex)
            {
                await LogWarning(context, ex);
                await CreateApiErrorResponse(context, ex);
            }
        }

        private async Task LogWarning(HttpContext httpContext, ApiException ex)
        {
            await _log.WriteWarningAsync(_componentName, httpContext.Request.GetUri().AbsoluteUri, null, ex.Message);
        }

        private async Task CreateApiErrorResponse(HttpContext ctx, ApiException ex)
        {
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)ex.StatusCode;

            var responseJson = JsonConvert.SerializeObject(new
            {
                ex.Message
            });

            await ctx.Response.WriteAsync(responseJson);
        }
    }
}
