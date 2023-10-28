using CleanLog.LogCategories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;
using System.Reflection;

namespace OmidBank.Common.Log
{
    public class CleanLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CleanLogMiddleware> _logger;

        public CleanLogMiddleware(RequestDelegate next, ILogger<CleanLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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

            var requestLog = new RequestLog
            (
             applicationName,
             traceId,
             clientIp.ToString(),
             requestPath,
             version.ToString() ?? string.Empty,
             controllerName.ToString() ?? string.Empty,
             actionName.ToString() ?? string.Empty,
             requestHttpMethod,
             DateTime.Now,
             requestBody,
             responseBodyText,
             stopwatch.ElapsedMilliseconds.ToString()
            );
            _logger.LogInformation($"RequestLog: {requestLog}");
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
        public static void UseCleanLog(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<CleanLogMiddleware>();
        }
    }
}