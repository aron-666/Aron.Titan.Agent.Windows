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

namespace Aron.Titan.Agent.Windows.Extensions
{
    public static class ServiceExtension
    {
        public static IHostBuilder AddApplication(this IHostBuilder builder)
        {
            builder.ConfigureServices((builder, services) =>
            {

                Config.Config config = new Config.Config();
                builder.Configuration.GetSection("Config").Bind(config);
                services.AddSingleton(config);

                // 註冊你的服務和依賴項
                services.AddSingleton<Form1>();

                

                // 註冊工廠
                //services.AddSingleton<IReaderWebSocketServiceFactory, ReaderWebSocketServiceFactory>();

                
            });
            return builder;
        }


        public static IHostBuilder AddLog(this IHostBuilder builder)
        {
            var storage = new { Path = Directory.GetCurrentDirectory() };


            var nlogConf = File.ReadAllText(Path.Combine(storage.Path, "NLog_Template.config"));

            string basedir = Path.Combine(storage.Path).Replace("\\", "/");
            if (basedir.EndsWith("/"))
                basedir = basedir.Substring(0, basedir.Length - 1);
            nlogConf = nlogConf.Replace("${basedir}", basedir);

            File.WriteAllText(Path.Combine(storage.Path, "NLog.config"), nlogConf);

            if (!Directory.Exists(Path.Combine(storage.Path, "logs")))
                Directory.CreateDirectory(Path.Combine(storage.Path, "logs"));

            builder.ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                logging.AddConsole();
                logging.AddNLog(Path.Combine(storage.Path, "NLog.config"));
            });


            return builder;

        }
    }
}
