namespace CleanLog
{
    public interface ICleanLogger
    {
        Task ErrorLog(string errorMessage);
        Task ProxyLog(string actionName, string httpMethod, string requestBody, string responseBody, string responseTime);
    }
}