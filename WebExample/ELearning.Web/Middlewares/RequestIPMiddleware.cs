using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearning.Web.Middlewares
{
    /// <summary>
    /// 获取访问 IP 的中间件
    /// </summary>
    public class RequestIPMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestIPMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("logName");
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("用户的 IP: " + context.Connection.RemoteIpAddress.ToString());
            await _next.Invoke(context);
        }
    }
}
