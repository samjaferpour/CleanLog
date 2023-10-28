namespace CleanLog.LogCategories
{
    public sealed class RequestLog
    {
        public string ApplicationName { get; set; }
        public string TraceId { get; set; }
        public string ClientIp { get; set; }
        public string RequestPath { get; set; }
        public string Version { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string RequestHttpMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public string ResponseTime { get; set; }
        public RequestLog(string applicationName, string traceId, string clientIp, string requestPath, string version, string controllerName, string actionName, string requestHttpMethod, DateTime createdAt, string requestBody, string responseBody, string responseTime) 
        {
            this.ApplicationName = applicationName;
            this.TraceId = traceId;
            this.ClientIp = clientIp;
            this.RequestPath = requestPath;
            this.Version = version;
            this.ControllerName = controllerName;
            this.ActionName = actionName;
            this.RequestHttpMethod = requestHttpMethod;
            this.CreatedAt = createdAt;
            this.RequestBody = requestBody;
            this.ResponseBody = responseBody;
            this.ResponseTime = responseTime;
        }
        public override string ToString()
        {
            return $"TraceId: {TraceId} - ClientIp: {ClientIp} - RequestPath: {RequestPath} - Version: {Version} - ControllerName: {ControllerName} - ActionName: {ActionName} - RequestHttpMethod: {RequestHttpMethod} - CreatedAt: {CreatedAt} - RequestBody: {RequestBody} - ResponseBody: {ResponseBody} - ResponseTime: {ResponseTime} ";
        }
    }
}
