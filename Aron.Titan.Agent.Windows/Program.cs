using Aron.Titan.Agent.Windows.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Aron.Titan.Agent.Windows
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
            // set application current directory
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                Environment.CurrentDirectory = AppContext.BaseDirectory;
            }
            IHostBuilder builder = Host
                .CreateDefaultBuilder()
                .AddLog()
                .AddApplication()
                .UseConsoleLifetime();



            var host = builder
                .Build();

            host.Start();
            Application.ApplicationExit += (_, _) => host.StopAsync().GetAwaiter().GetResult();
            ApplicationConfiguration.Initialize();

            Application.Run(host.Services.GetRequiredService<Form1>());
            host.WaitForShutdown();
        }

    }
}