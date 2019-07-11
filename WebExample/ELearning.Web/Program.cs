using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ELearning.DataAccess.Seeds;
using ELearning.ORM.SqlServer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ELearning.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<SqlServerDbContext>();
                try
                {
                    // 这里执行相关的种子数据处理代码
                    ApplicationDataSeed.ForEntities(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "在创建数据种子数据过程中，发生了错误。");
                }
            }

            host.Run();
        }

        /// <summary>
        /// 构建 APP 驻留实例的方法：
        /// ASP.Net Core 2.x 使用 WebHost 来创建 WebApp 驻留实例，通过 CreateDefaultBuilder 方法
        /// 使用常规的环境选项（在 Stardup 中配置）设置驻留环境，基本的流程如下：
        ///    1. 使用 Kestrel 作为 Web 服务器以便于 IIS 集成；
        ///    2. 从 appsetting.josn 文件中加载环境变量、命令行参数以及其它资源；
        ///    3. 向控制台等调试器输出日志信息。
        /// </summary>
        /// <param name="args">命令行参数集合</param>
        /// <returns>构建的驻留实例</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
