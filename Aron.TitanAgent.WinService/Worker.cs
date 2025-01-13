using System.Buffers.Text;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace Aron.TitanAgent.WinService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Settings settings;
    Process? process = null;
    string? workingDir, serverUrl, key;


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
                        workingDir = Environment.GetEnvironmentVariable("TITAN_AGENT_WORKING_DIR", EnvironmentVariableTarget.Machine);
                        serverUrl = Environment.GetEnvironmentVariable("TITAN_AGENT_SERVER_URL", EnvironmentVariableTarget.Machine);
                        key = Environment.GetEnvironmentVariable("TITAN_AGENT_KEY", EnvironmentVariableTarget.Machine);

                        if (string.IsNullOrEmpty(workingDir) || string.IsNullOrEmpty(serverUrl) || string.IsNullOrEmpty(key))
                        {
                            // �g���~��ƥ��˵�

                            using (EventLog eventLog = new EventLog("Application"))
                            {
                                eventLog.Source = "Titan Agent";
                                eventLog.WriteEntry("�����ܼƥ��]�m", EventLogEntryType.Error, 101, 1);
                            }
                            _logger.LogError("�����ܼƥ��]�m", "An error occurred.");

                            return;
                        }
                        string programPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "TitanNetwork", "Agent");
                        string exePath = Path.Combine(programPath, "agent.exe");
                        string arguments = $" --working-dir=\"{workingDir}\" --server-url=\"{serverUrl}\" --key=\"{key}\"";
                        AuthMultipass();

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
                    // �g���~��ƥ��˵�
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Titan Agent";
                        eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error, 100, 1);
                    }

                    _logger.LogError(ex, "An error occurred.");
                }
                finally
                {
                    CloseProcess();
                }
            }
        }


    }

    private void AuthMultipass()
    {
        try
        {
            
            string base64Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(key ?? "abc"));

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "multipass",
                Arguments = " auth " + base64Key,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = workingDir,
            };
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(output))
            {
                _logger.LogInformation(output);
            }

            if (!string.IsNullOrEmpty(error))
            {
                _logger.LogError(error);
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred.");
        }
    }

    private static int GetParentProcessId(int processId)
    {
        var query = $"SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {processId}";
        var searcher = new ManagementObjectSearcher(query);
        var queryCollection = searcher.Get();

        foreach (var process in queryCollection)
        {
            return Convert.ToInt32(process["ParentProcessId"]);
        }

        return -1; // �L�k�����i�{
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Service is stopping. Performing cleanup...");

        // �b�o�̲K�[�M�z�޿�A�Ҧp����귽
        CloseProcess();

        await Task.CompletedTask;

        _logger.LogInformation("Service cleanup completed.");
    }

    private void CloseProcess()
    {
        if (process != null)
        {
            var processes = Process.GetProcesses();
            var childProcesses = processes.Where(p => GetParentProcessId(p.Id) == process.Id);

            
            foreach (var p in childProcesses)
            {
                try
                {
                    p.Kill();
                    p.Dispose();
                }
                catch
                {

                }

            }

            
            process.Kill();
            process.Dispose();

            process = null;
            if(!string.IsNullOrEmpty(workingDir))
            {
                var controllerProcesses = processes.Where(p =>
                {
                    try
                    {
                        return p.MainModule.FileName == Path.Combine(workingDir, "A", "controller.exe")
                            || p.MainModule.FileName == Path.Combine(workingDir, "B", "controller.exe");
                    }
                    catch
                    {
                        return false;
                    }
                });

                foreach (var p in controllerProcesses)
                {
                    try
                    {
                        p.Kill();
                        p.Dispose();
                    }
                    catch
                    {
                    }
                }
            }
            
        }
    }
}
