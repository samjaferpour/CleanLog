﻿using CleanLog.LogCategories;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;
using System.Reflection;

namespace CleanLog
{
    public class CleanLogger : ICleanLogger
    {
        private readonly IHttpContextAccessor _accessor;
        private Serilog.ILogger _logger { get; set; }
        private DateTime loggerDateTime;
        private const string RootFolder = "logs/";
        private const string LogFileName = "/log-.log";
        private const string LogFolderFormat = "yyyy-MM-dd";
        private const RollingInterval RollingType = RollingInterval.Day;
        private const long FileSizeLimitBytes = 1048576;
        private const bool RollOnFileSizeLimit = true;

        public CleanLogger(IHttpContextAccessor accessor)
        {
            _accessor = accessor;

            _logger = new LoggerConfiguration()

                .WriteTo.Map(le => loggerDateTime.ToString(LogFolderFormat
                ), (folderName, wt) => wt.File(AppDomain.CurrentDomain.BaseDirectory + RootFolder + $"{folderName}" + LogFileName, rollingInterval: RollingType, fileSizeLimitBytes: FileSizeLimitBytes, rollOnFileSizeLimit: RollOnFileSizeLimit, retainedFileCountLimit: null))
                .CreateLogger();

        }
        public async Task ProxyLog(string actionName, string httpMethod, string requestBody, string responseBody, string responseTime)
        {
            var traceId = _accessor.HttpContext.TraceIdentifier;
            var proxyLog = new ProxyLog
                (
                    traceId,
                    actionName,
                    httpMethod,
                    requestBody,
                    responseBody,
                    DateTime.Now,
                    responseTime
                );
            _logger.Information($"Proxylog: {proxyLog}");
        }
        public async Task ErrorLog(string errorMessage)
        {
            var traceId = _accessor.HttpContext.TraceIdentifier;
            var errorLog = new ErrorLog
                (
                    traceId,
                    errorMessage,
                    DateTime.Now
                );
            _logger.Error($"ErrorLog: {errorLog}");
        }

        public async Task RequestLog(string applicationName, string traceId, string clientIp, string requestPath, string version, string controllerName, string actionName, string requestHttpMethod, DateTime createdAt, string requestBody, string responseBody, string responseTime)
        {
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
             responseBody,
             responseTime
            );
            _logger.Information($"RequestLog: {requestLog}");
        }
    }
}
