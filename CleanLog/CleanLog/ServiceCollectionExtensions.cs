using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CleanLog
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLoggerLibrary(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICleanLogger, CleanLogger>();
        }
    }
}
