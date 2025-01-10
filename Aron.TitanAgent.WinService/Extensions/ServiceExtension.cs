using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NLog.Config;
using NLog.Targets;
using NLog;

namespace Aron.TitanAgent.WinService.Extensions
{
    public static class ServiceExtension
    {
        public static IHostBuilder AddLog(this IHostBuilder builder, string path)
        {
            builder.ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                logging.AddConsole();
                var config = ConfigureNLog(path);
                logging.AddNLog(config);
            });


            return builder;

        }

        private static LoggingConfiguration ConfigureNLog(string logDirectory)
        {
            // 創建 NLog 配置
            var config = new LoggingConfiguration();

            // 定義文件目標
            var fileTarget = new FileTarget("file")
            {
                FileName = $"{logDirectory}/Logs/${{shortdate}}.txt",
                Layout = "${longdate} ${uppercase:${level}} ${message} ${exception}",
                KeepFileOpen = false,
            };

            // 定義主控台目標
            var consoleTarget = new ConsoleTarget("console")
            {
                Layout = "${longdate} ${uppercase:${level}} ${message} ${exception}",
            };

            // 將目標添加到配置
            config.AddTarget(fileTarget);
            config.AddTarget(consoleTarget);

            // 定義日誌規則
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, fileTarget);
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, consoleTarget);

            // 應用配置
            return config;
        }
    }
}
