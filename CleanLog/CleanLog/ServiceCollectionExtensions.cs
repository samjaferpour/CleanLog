﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OmidBank.Common.Log;

namespace CleanLog
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCleanLog(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICleanLogger, CleanLogger>();
        }
    }
}
