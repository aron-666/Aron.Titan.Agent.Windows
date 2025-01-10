using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Aron.TitanAgent.WinService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Settings settings;
    Process? process = null;


    public Worker(ILogger<Worker> logger, Settings settings)
    {
        _logger = logger;
        this.settings = settings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        List<ErrorRecord> errorLogs = new List<ErrorRecord>();
        string? stdOutError = null;
        string[] args = settings.Args;
        IntPtr? jobHandle = null;
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

                        if (string.IsNullOrEmpty(workingDir) || string.IsNullOrEmpty(serverUrl) || string.IsNullOrEmpty(key))
                        {
                            // 寫錯誤到事件檢視

                            using (EventLog eventLog = new EventLog("Application"))
                            {
                                eventLog.Source = "Titan Agent";
                                eventLog.WriteEntry("環境變數未設置", EventLogEntryType.Error, 101, 1);
                            }
                            _logger.LogError("環境變數未設置", "An error occurred.");

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
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = false,
                            WorkingDirectory = programPath,
                        };

                        process = new Process();
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                _logger.LogInformation(e.Data);
                            }
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {

                                if (stdOutError == null)
                                {
                                    _logger.LogError(e.Data);
                                    errorLogs.Add(new ErrorRecord() { Message = e.Data, Time = DateTime.Now });

                                    // 檢查重複錯誤，3秒內超過5次，寫入stdOutError
                                    // group by 3秒內的錯誤
                                    var group = errorLogs
                                        .Where(x => x.Time >= DateTime.Now.AddSeconds(-3))
                                        .GroupBy(x => x.Message);
                                    foreach (var item in group)
                                    {
                                        if (item.Count() > 5)
                                        {
                                            stdOutError = item.First().Message;
                                            errorLogs.Clear();
                                            break;
                                        }
                                    }
                                    errorLogs.RemoveAll(x => x.Time < DateTime.Now.AddSeconds(-3));
                                }
                                else
                                {
                                    if (e.Data != stdOutError)
                                    {
                                        _logger.LogError(e.Data);
                                    }
                                }
                            }
                        };
                        process.StartInfo = startInfo;
                        process.Start();

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        using (EventLog eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Titan Agent";
                            eventLog.WriteEntry("Titan Agent Start", EventLogEntryType.Information, 100, 1);
                            _logger.LogInformation("Titan Agent Start");

                            await process.WaitForExitAsync(stoppingToken);

                            eventLog.WriteEntry("Titan Agent Stop", EventLogEntryType.Information, 100, 1);
                            _logger.LogInformation("Titan Agent Stop");
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

                    _logger.LogError(ex, "An error occurred.");
                }
                finally
                {
                    if (process != null)
                    {
                        process.Kill();
                        process.Dispose();
                        process = null;
                    }
                }
            }
        }


    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Service is stopping. Performing cleanup...");

        // 在這裡添加清理邏輯，例如釋放資源
        if (process != null)
        {
            process.Kill();
            process.Dispose();
            process = null;
        }

        await Task.CompletedTask;

        _logger.LogInformation("Service cleanup completed.");
    }
}
