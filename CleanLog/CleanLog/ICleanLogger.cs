namespace CleanLog
{
    public interface ICleanLogger
    {
        Task RequestLog(string applicationName, string traceId, string clientIp, string requestPath, string version, string controllerName, string actionName, string requestHttpMethod, DateTime createdAt, string requestBody, string responseBody, string responseTime);
        Task ProxyLog(string actionName, string httpMethod, string requestBody, string responseBody, string responseTime);
        Task ErrorLog(string errorMessage);
    }
}