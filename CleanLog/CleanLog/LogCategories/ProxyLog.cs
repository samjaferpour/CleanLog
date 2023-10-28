namespace CleanLog.LogCategories
{
    public sealed class ProxyLog
    {
        public string TraceId { get; set; }
        public string ActionName { get; set; }
        public string HttpMethod { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ResponseTime { get; set; }
        public ProxyLog(string traceId, string actionName, string httpMethod, string requestBody, string responseBody, DateTime createdAt, string responseTime) 
        {
            this.TraceId = traceId;
            this.ActionName = actionName;
            this.HttpMethod = httpMethod;
            this.RequestBody = requestBody;
            this.ResponseBody = responseBody;
            this.CreatedAt = createdAt;
            this.ResponseTime = responseTime;
        }
        public override string ToString()
        {
            return $"TraceId: {TraceId} - ActionName: {ActionName} - HttpMethod: {HttpMethod} - RequestBody: {RequestBody} - ResponseBody: {ResponseBody} - CreatedAt: {CreatedAt} - ResponseTime: {ResponseTime} ";
        }
    }
}
