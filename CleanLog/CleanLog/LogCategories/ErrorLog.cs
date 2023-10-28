namespace CleanLog.LogCategories
{
    public sealed class ErrorLog
    {
        public string TraceId { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public ErrorLog(string traceId, string errorMessage, DateTime createdAt)
        {
            this.TraceId = traceId;
            this.ErrorMessage = errorMessage;
            this.CreatedAt = createdAt;
        }
        public override string ToString()
        {
            return $"TraceId: {TraceId} - ErrorMessage:{ErrorMessage} - CreatedAt: {CreatedAt} ";
        }
    }
}
