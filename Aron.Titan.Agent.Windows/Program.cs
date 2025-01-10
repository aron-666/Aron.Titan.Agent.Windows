using Aron.Titan.Agent.Windows.Extensions;
using MaterialSkin.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Aron.Titan.Agent.Windows
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
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

            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    try
                    {
                        if (arg.Equals("--start-service", StringComparison.OrdinalIgnoreCase))
                        {
                            string? workingDir = Environment.GetEnvironmentVariable("TITAN_AGENT_WORKING_DIR", EnvironmentVariableTarget.Machine);
                            string? serverUrl = Environment.GetEnvironmentVariable("TITAN_AGENT_SERVER_URL", EnvironmentVariableTarget.Machine);
                            string? key = Environment.GetEnvironmentVariable("TITAN_AGENT_KEY", EnvironmentVariableTarget.Machine);
                            string? logFile = Environment.GetEnvironmentVariable("LOG_FILE", EnvironmentVariableTarget.Machine);

                            if (string.IsNullOrEmpty(workingDir) || string.IsNullOrEmpty(serverUrl) || string.IsNullOrEmpty(key))
                            {
                                // 寫錯誤到事件檢視

                                using (EventLog eventLog = new EventLog("Application"))
                                {
                                    eventLog.Source = "Titan Agent";
                                    eventLog.WriteEntry("環境變數未設置", EventLogEntryType.Error, 101, 1);
                                }

                                return;
                            }
                            string programPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "TitanNetwork", "Agent");
                            string exePath = Path.Combine(programPath, "agent.exe");
                            string arguments = $" --working-dir=\"{workingDir}\" --server-url=\"{serverUrl}\" --key=\"{key}\"";
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                FileName = exePath,
                                Arguments = arguments,
                                UseShellExecute = false,
                                RedirectStandardOutput = false,
                                RedirectStandardError = false,
                                CreateNoWindow = false,
                                WorkingDirectory = programPath,

                            };
                            startInfo.EnvironmentVariables["LOG_FILE"] = logFile;
                            using (Process process = new Process())
                            {
                                process.StartInfo = startInfo;
                                process.Start();
                                process.WaitForExit();
                            }
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        // 寫錯誤到事件檢視
                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Titan Agent";
                            eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error, 100, 1);
                        }
                    }
                    
                    
                }
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