using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using NLog.Targets;

#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010

namespace SemanticKernel_Demo.AIRegister
{

    public static class NLogServiceCollectionExtensions
    {
        /// <summary>
        /// 添加NLog日志服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        /// <remarks>此方法用于将NLog配置添加到依赖注入容器中</remarks>
        /// <example>
        /// services.AddNLogging();
        /// </example>
        /// <see cref="NLogServiceCollectionExtensions.AddNLogging(IServiceCollection)"/>

        public static IServiceCollection AddNLogging(this IServiceCollection services)
        {
            // 定义文件日志输出目标
            var fileTarget = new FileTarget()
            {
                FileName = "${basedir}/logs/sk-demo-${shortdate}.log", // 按日期每日生成一份日志文件
                AutoFlush = true,
                Encoding = System.Text.Encoding.UTF8,
                DeleteOldFileOnStartup = false // 保留历史日志
            };
            // 定义控制台日志输出目标
            var consoleTarget = new ConsoleTarget();

            var config = new NLog.Config.LoggingConfiguration();
            // 定义日志输出规则(输出所有Trace级别及以上的日志到控制台)
            config.AddRule(
                NLog.LogLevel.Trace,
                NLog.LogLevel.Fatal,
                target: fileTarget,  // 这里采用文件输出
                "*");// * 表示所有Logger
                     // 注册NLog
            services.AddLogging(loggingBuilder => loggingBuilder.AddNLog(config));
            return services;
        }
    }
}
