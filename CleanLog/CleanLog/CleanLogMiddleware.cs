using CleanLog;
using CleanLog.LogCategories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Serilog;
using System.Diagnostics;
using System.Reflection;

namespace CleanLog
{
    public class CleanLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICleanLogger _cleanLogger;

        public CleanLogMiddleware(RequestDelegate next, ICleanLogger cleanLogger)
        {
            _next = next;
            _cleanLogger = cleanLogger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            var applicationName = entryAssembly.GetName().Name;
            var traceId = context.TraceIdentifier;
            var clientIp = context.Connection.RemoteIpAddress;
            var requestPath = context.Request.Path;
            var version = context.GetRouteData().Values["version"];
            var controllerName = context.GetRouteData().Values["controller"];
            var actionName = context.GetRouteData().Values["action"];
            var requestHttpMethod = context.Request.Method;
            var createdAt = DateTime.Now;

            // Read the request body content
            var requestBody = await ReadBodyFromRequest(context.Request);
            HttpResponse response = context.Response;
            var originalResponseBody = response.Body;
            using var newResponseBody = new MemoryStream();
            response.Body = newResponseBody;

            await _next(context);

            newResponseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(response.Body).ReadToEndAsync();

            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);

            stopwatch.Stop();

            await _cleanLogger.RequestLog(applicationName ?? string.Empty,
                                    traceId,clientIp.ToString(),
                                    requestPath,
                                    version.ToString() ?? string.Empty,
                                    controllerName.ToString() ?? string.Empty,
                                    actionName.ToString() ?? string.Empty,
                                    requestHttpMethod,
                                    DateTime.Now,
                                    requestBody,
                                    responseBodyText,
                                    stopwatch.ElapsedMilliseconds.ToString());
        }
        #region private methods
        private async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            request.EnableBuffering();
            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();
            request.Body.Position = 0;
            return requestBody;
        }
        #endregion
    }


    public static class RequestLogMiddlewareExtension
    {
        public static IApplicationBuilder UseCleanLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CleanLogMiddleware>();
        }
    }
}